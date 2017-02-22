using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class SaturationController : MonoBehaviour {
		public SpriteRenderer Renderer;
		private Color _startColor;

		// Use this for initialization
		void Start () {
			_startColor = Renderer.color;
		}
		
		// Update is called once per frame
		void Update () {
			float h, s, v;

			Color.RGBToHSV(_startColor, out h, out s, out v);

			v = PlayerStats.ForestSaturation;
			Renderer.color = Color.HSVToRGB(h, s, v);
		}
	}
}
