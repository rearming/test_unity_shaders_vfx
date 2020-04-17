using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

public class SharkMovement : MonoBehaviour
{
	[SerializeField] private float moveSpeed;
	[Tooltip("After distance to current target point will reach this value, next point will be chosen")]
	[SerializeField] private float distanceThreshold;
	[SerializeField] private float bezierCurveHeight;
	
	[SerializeField] private GameObject pointsContainer;
	[SerializeField] private Transform[] points;
	
	private Vector3[] pointsPositions;
	private float[] pointsDistances;
	private int currentPointIndex;

	private float moveLerpT;

	private void Start()
	{
		GetPointsAndDistances();
		transform.position = GetTargetPos(out var dummy);
	}

	private void GetPointsAndDistances()
	{
		if (pointsContainer != null)
			pointsPositions = pointsContainer.GetComponentsInChildren<Transform>().
				Where(t => t.parent != null).Select(t => t.position).ToArray();
		else
			pointsPositions = points.Select(t => t.position).ToArray();
		pointsDistances = pointsPositions.Select((p, i)
			=> Vector3.Distance(p, pointsPositions[GetNextIndexWrapped(i)])).ToArray();
	}

	private void Update()
	{
		var fixedMoveSpeed = moveSpeed * Time.deltaTime;
		var targetPos = GetTargetPos(out var currentDistance);
		var prevPos = pointsPositions[GetPrevIndexWrapped(currentPointIndex)];
		
		// var targetRotation = Quaternion.LookRotation(
			// pointsPositions[GetNextIndexWrapped(currentPointIndex)] - transform.position);
		// var rotationStep = 1 / pointsDistances[currentPointIndex];
		// var lerpT = currentDistance * rotationStep;
		// transform.position = Vector3.MoveTowards(transform.position, targetPos, fixedMoveSpeed);
		
		var moveStepSize = 1 / pointsDistances[currentPointIndex];
		
		transform.position =
			GetPointOnBezierCurve(prevPos, targetPos, GetBezierMiddlePoint(prevPos, targetPos), moveLerpT);

		var targetRot = Quaternion.LookRotation(transform.position - prevPos);
		
		// Debug.Log(lerpT.ToString());
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, moveLerpT);

		moveLerpT += fixedMoveSpeed * moveStepSize;
	}

	private Vector3 GetBezierMiddlePoint(Vector3 prevPos, Vector3 targetPos)
	{
		var halfVec = (targetPos - prevPos) / 2 + prevPos;
		return halfVec + halfVec.normalized * bezierCurveHeight;
	}
	
	private Vector3 GetPointOnBezierCurve(Vector3 current, Vector3 target, Vector3 offset, float t)
	{
		t = Mathf.Clamp01(t);
		float oneMinusT = 1f - t;
		return
			oneMinusT * oneMinusT * current +
			2f * oneMinusT * t * offset +
			t * t * target;
	}
	
	private Vector3 GetTargetPos(out float currentDistance)
	{
		currentDistance = Vector3.Distance(transform.position, pointsPositions[currentPointIndex]);
			// можно заменить проверкой на lerpValue >= 1
		
		if (currentDistance <= distanceThreshold)
		{
			currentPointIndex = GetNextIndexWrapped(currentPointIndex);
			moveLerpT = 0f;
		}
		return pointsPositions[currentPointIndex];
	}

	#region Array access utils

	private int GetNextIndexWrapped(int current, int length)
	{
		return (current + 1) % length;
	}
	
	private int GetNextIndexWrapped(int current)
	{
		return (current + 1) % pointsPositions.Length;
	}

	private int GetPrevIndexWrapped(int current, int length)
	{
		return current > 0 ? current - 1 : length - 1;
	}
	
	private int GetPrevIndexWrapped(int current)
	{
		return current > 0 ? current - 1 : pointsPositions.Length - 1;
	}
	
	#endregion
	
}
