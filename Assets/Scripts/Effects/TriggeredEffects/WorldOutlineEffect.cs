using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Detection
{
	public class WorldOutlineEffect : MonoBehaviour, IEffect
	{
		private List<Vector3> pointList;
		private Transform playerTransform;
		[SerializeField] private int numPoints = 300;
		private float maxDistance;
		private LayerMask layerMask;

		[SerializeField] private int weight = 5;

		public int Weight { get; set; }

		private float lineStartThickness = 0.02f;
		private float lineEndThickness = 0.02f;
		[SerializeField] private Material lineMaterial;
		[SerializeField] private Color startingColor;
		[SerializeField] private Color endingColor;

		private LineRenderer line;

		public void Awake()
		{
			Weight = weight;
            maxDistance = 20;
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

            pointList = new List<Vector3>();
            layerMask = LayerMask.GetMask("Environment");

			line = gameObject.AddComponent<LineRenderer>();
		}

		void IEffect.DoEffect(Action callback)
		{
			float duration = .5f;

			int numPointsHit = 0;
			// Shoot rays 360 degrees around the player and set point data
			float angle = 0;
			for (int i = 0; i < numPoints; i++)
			{
				float x = Mathf.Cos(angle);
				float z = Mathf.Sin(angle);

				Vector3 dir = new Vector3(playerTransform.position.x * x, 0, playerTransform.position.z * z);
				Vector3 fwd = transform.TransformDirection(dir);

				Vector3 playerPosOffset = playerTransform.position;
				playerPosOffset.y += 0.15f;

				RaycastHit rayHit;
				if (Physics.Raycast(playerPosOffset, dir, out rayHit, maxDistance, layerMask))
				{
					numPointsHit++;
					pointList.Add(rayHit.point);
				}

                angle += 2 * Mathf.PI / numPoints;
			}

			line.startWidth = lineStartThickness;
			line.endWidth = lineEndThickness;
			line.positionCount = numPointsHit;
			line.startColor = startingColor;
			line.endColor = startingColor;
			line.material = lineMaterial;
			line.SetPositions(pointList.ToArray());

			/*
			bool createdNewLineRenderer = true;
			for (int i = 0; i < numPointsHit; i++)
			{
				// Update the line data if.... 
				// 1. both points have hit something.
				// 2. the distance in between two points are less than the tolerance.
				if (i > 0 && (pointList[i].GetHasHitWall() && pointList[i - 1].GetHasHitWall()))
				{
					if (Vector3.Distance(pointList[i].GetLocation(), pointList[i - 1].GetLocation()) < diffDistanceTolerance)
                    {
						line.SetPosition(i, pointList[i].location);
					}
				}
			}*/

			StartCoroutine(FadeLineColor(startingColor, endingColor, duration, callback));
		}

		public IEnumerator FadeLineColor(Color startColor, Color endColor, double duration, Action callback)
		{
			for (float t = 0f; t < duration; t += Time.deltaTime)
			{
				float normalizedTime = t / (float)duration;
				Color lerped = Color.Lerp(startColor, endColor, normalizedTime);
				lerped.a = Mathf.Lerp(lerped.a, 0, normalizedTime);
				line.startColor = lerped;
				line.endColor = lerped;
				yield return null;
			}

			line.positionCount = 0;
			line.startColor = endColor;
			line.endColor = endColor;
			pointList.Clear();

			callback();
		}
	}
}
