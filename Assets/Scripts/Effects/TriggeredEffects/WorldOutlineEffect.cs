using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Detection
{
	public class WorldOutlineEffect : MonoBehaviour, IEffect
	{
		public struct OutlinePoint
		{
			private bool hasHitWall;
			private Vector3 location;

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
		private int layerMask;

		private Color curColor;

		public void Initialize(MusicAnalyzer mAnalyzer, GameObject player, int newNumPoints, float newMaxDistance)
		{
			musicAnalyzer = mAnalyzer;
			numPoints = newNumPoints;
			maxDistance = newMaxDistance;
			playerObj = player;

			pointList = new List<OutlinePoint>();

			for (int i = 0; i < numPoints; i++)
				pointList.Add(new OutlinePoint(false, new Vector3(0, 0, 0)));
		}

		void IEffect.DoEffect(double duration, Action callback)
		{
			// Shoot rays 360 degrees around the player and set point data
			float angle = 0;
			for (int i = 0; i < numPoints; i++)
			{
				if (!pointList[i].GetHasHitWall())
				{
					float x = Mathf.Sin(angle);
					float y = Mathf.Cos(angle);
					angle += 2 * Mathf.PI / numPoints;

					Vector3 dir = new Vector3(playerObj.transform.position.x * x, playerObj.transform.position.y * y, 0);
					//Vector3 fwd = transform.TransformDirection(dir); // is this necessary? maybe just use dir?
					RaycastHit rayHit;

					if (Physics.Raycast(playerObj.transform.position, dir, out rayHit, maxDistance))
					{
						pointList[i].SetLocation(rayHit.point);
						pointList[i].SetHasHitWall(true);
					}
					else pointList[i].SetHasHitWall(false);
				}
			}

			Color startColor = new Color(255, 255, 255, 255);
			Color endColor = new Color(0, 0, 0, 0);
			StartCoroutine(DoWorldOutlineEffect(startColor, endColor, duration, callback));
		}

		public IEnumerator DoWorldOutlineEffect(Color startColor, Color endColor, double duration, Action callback)
		{
			for (float t = 0f; t < duration; t += Time.deltaTime)
			{
				float normalizedTime = t / (float)duration;
				curColor = Color.Lerp(startColor, endColor, normalizedTime);

				// Draw all the points
				for (int i = 0; i < numPoints; i++)
				{
					if (i + 1 < numPoints)
						Debug.DrawLine(pointList[i].GetLocation(), pointList[i + 1].GetLocation(), curColor);
				}

				yield return null;
			}

			curColor = endColor;

			callback();
		}

		/*

		IEnumerator ChangePointColorOverTime(Color start, Color end, float duration)
		{
			for (float t = 0f; t < duration; t += Time.deltaTime)
			{
				float normalizedTime = t / duration;
				//right here, you can now use normalizedTime as the third parameter in any Lerp from start to end
				someColorValue = Color.Lerp(start, end, normalizedTime);
				yield return null;
			}
			someColorValue = end; //without this, the value will end at something like 0.9992367
		}

		*/

	}
}
