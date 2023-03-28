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
		private Transform playerTransform;
		private int numPoints;
		private float maxDistance;
		private LayerMask layerMask;
		private float diffDistanceTolerance;
        public int Weight { get; set; }

		[SerializeField] private float lineStartThickness;
		[SerializeField] private float lineEndThickness;

		public void Awake()
		{
			Weight = 5;
		}

		public void Initialize(MusicAnalyzer mAnalyzer, GameObject player, int newNumPoints, float newMaxDistance, float newDiffDistanceTolerance)
		{
			musicAnalyzer = mAnalyzer;
			numPoints = newNumPoints;
			maxDistance = newMaxDistance;
			playerTransform = player.transform;
			diffDistanceTolerance = newDiffDistanceTolerance;

			pointList = new List<OutlinePoint>();

			layerMask = LayerMask.GetMask("Environment");

			InitializePointList();
		}

		void InitializePointList()
        {
			pointList.Clear();

			for (int i = 0; i < numPoints; i++)
				pointList.Add(new OutlinePoint(false, new Vector3(0, 0, 0)));
		}

		void IEffect.DoEffect(Action callback)
		{
			float duration = 1f;

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

					Vector3 dir = new Vector3(playerTransform.position.x * x, 0, playerTransform.position.z * z);
					Vector3 fwd = transform.TransformDirection(dir); // is this necessary? maybe just use dir?

					RaycastHit rayHit;

					Vector3 playerPosOffset = playerTransform.position;
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

			Color startingColor = new Color(255, 255, 255, 255);

			//Create the line renderer to draw the points.
			LineRenderer line = new LineRenderer();
			line.startWidth = lineStartThickness;
			line.endWidth = lineEndThickness;
			line.positionCount = numPointsHit;
			line.startColor = startingColor;
			line.endColor = startingColor;

			line.SetPosition(0, new Vector3(0, 0, 0));
			line.SetPosition(1, new Vector3(1, 1, 1));
			line.SetPosition(3, new Vector3(2, 2, 2));
			line.SetPosition(4, new Vector3(3, 3, 3));

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
			Color endingColor = new Color(0, 0, 0, 0);
			StartCoroutine(FadeLineColor(line, startingColor, endingColor, duration, callback));
		}

		public IEnumerator FadeLineColor(LineRenderer line, Color startColor, Color endColor, double duration, Action callback)
		{
			for (float t = 0f; t < duration; t += Time.deltaTime)
			{
				float normalizedTime = t / (float)duration;
				Color lerped = Color.Lerp(startColor, endColor, normalizedTime);
				line.startColor = lerped;
				line.endColor = lerped;
				yield return null;
			}

			line.startColor = endColor;
			line.endColor = endColor;
			InitializePointList();

			callback();
		}
	}
}
