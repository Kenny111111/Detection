using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Detection
{
	public class WorldOutlineEffect : MonoBehaviour, IEffect
	{
		public class OutlinePoint
		{
			public bool hasHitWall;
			public Vector3 location;

			public OutlinePoint(bool newHitWall, Vector3 newLocation)
			{
				this.hasHitWall = newHitWall;
				this.location = newLocation;
			}

			public bool GetHasHitWall()
			{
				return this.hasHitWall;
			}
			public void SetHasHitWall(bool newHasHitWall)
            {
				this.hasHitWall = newHasHitWall;
			}

			public Vector3 GetLocation()
			{
				return this.location;
			}
			public void SetLocation(Vector3 newLocation)
			{
				this.location = newLocation;
			}
		}

		public MusicAnalyzer musicAnalyzer;
		private List<OutlinePoint> pointList;
		private GameObject playerObj;
		private int numPoints;
		private float maxDistance;
		private LayerMask layerMask;
		private float diffDistanceTolerance;
		private LinesManager linesManager;

		public void Initialize(MusicAnalyzer mAnalyzer, GameObject player, int newNumPoints, float newMaxDistance, float newDiffDistanceTolerance)
		{
			musicAnalyzer = mAnalyzer;
			numPoints = newNumPoints;
			maxDistance = newMaxDistance;
			playerObj = player;
			diffDistanceTolerance = newDiffDistanceTolerance;

			linesManager = FindObjectOfType<LinesManager>();

			pointList = new List<OutlinePoint>();

			layerMask = LayerMask.GetMask("Environment");

			InitPointList();
		}


		void InitPointList()
        {
			pointList.Clear();

			for (int i = 0; i < numPoints; i++)
				pointList.Add(new OutlinePoint(false, new Vector3(0, 0, 0)));
		}

		void IEffect.DoEffect(double duration, Action callback)
		{
			linesManager.Reset();

			int numPointsHit = 0;
			// Shoot rays 360 degrees around the player and set point data
			float angle = 0;
			for (int i = 0; i < numPoints; i++)
			{
				if (!pointList[i].GetHasHitWall())
				{
					float x = Mathf.Cos(angle);
					float z = Mathf.Sin(angle);
					angle += 2 * Mathf.PI / numPoints;

					Vector3 dir = new Vector3(playerObj.transform.position.x * x, 0, playerObj.transform.position.z * z);
					Vector3 fwd = transform.TransformDirection(dir); // is this necessary? maybe just use dir?

					RaycastHit rayHit;

					Vector3 playerPosOffset = playerObj.transform.position;
					playerPosOffset.y += 0.15f;

					if (Physics.Raycast(playerPosOffset, dir, out rayHit, maxDistance, layerMask))
					{
						numPointsHit++;
						pointList[i].SetLocation(rayHit.point);
						pointList[i].SetHasHitWall(true);
					}
					else pointList[i].SetHasHitWall(false);
				}
			}

			for (int i = 0; i < numPointsHit; i++)
			{
				// Update the line data if.... 
				// 1. both points have hit something.
				// 2. the distance in between two points are less than the tolerance.
				if (i > 0 && (pointList[i].GetHasHitWall() && pointList[i - 1].GetHasHitWall())
					&& (Vector3.Distance(pointList[i].GetLocation(), pointList[i - 1].GetLocation()) < diffDistanceTolerance))
				{
					linesManager.lines.Add(new KeyValuePair<Vector3, Vector3>(pointList[i - 1].location, pointList[i].location));
				}
			}

			Color startColor = new Color(255, 255, 255, 255);
			Color endColor = new Color(0, 0, 0, 0);
			StartCoroutine(FadeLineColor(startColor, startColor, duration, callback));
		}

		public IEnumerator FadeLineColor(Color startColor, Color endColor, double duration, Action callback)
		{
			duration = 1f;

			for (float t = 0f; t < duration; t += Time.deltaTime)
			{
				float normalizedTime = t / (float)duration;
				linesManager.color = Color.Lerp(startColor, endColor, normalizedTime);
				yield return null;
			}

			linesManager.color = endColor;
			InitPointList();

			callback();
		}
	}
}
