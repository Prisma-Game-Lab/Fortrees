using UnityEngine;

namespace Assets.Scripts
{
    public class Waypoints : MonoBehaviour {
        public static Transform[] Points;

        public void Awake()
        {
            Points = new Transform[transform.childCount];
            for (var i = 0; i < Points.Length; i++)
            {
                Points[i] = transform.GetChild(i);
            }
        }

    }
}
