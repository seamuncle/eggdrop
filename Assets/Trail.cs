using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TrailCurveType
{
    LINEAR,
    CATMULL_ROM
}

/**
 * Data representation of a path to be followed
 */
public class Trail : MonoBehaviour
{

    /**
	 * What type of curve is represented by the points
	 */
    public TrailCurveType curveType;

    /**
	 * A linear representation of points along a trail with first being start and last being end
	 */
    public List<Transform> points;

    /**
	 * Does path repeat?  A closed version of the path may be fundamentally different than an open one,
	 * depending on the curve
	 */
	public iTween.LoopType loopType;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Editor Stuff

    void OnDrawGizmos()
    {

        /*  iTween built in draws...
        switch (curveType)
        {
            case TrailCurveType.LINEAR:
                iTween.DrawLine(points.ToArray());
                iTween.DrawLineGizmos(points.ToArray());
                break;
            case TrailCurveType.CATMULL_ROM:
                iTween.DrawPath(points.ToArray());
                iTween.DrawPathGizmos(points.ToArray());

                break;
        }

         **/


        // Draw the points
        Color pointStartColor = new Color(1f, 0.25f, 0.25f);
        Color pointFinishColor = new Color(0.25f, 0.25f, 1f);
        new Color();
        for (int i = 0; i < points.Count; i++)
        {
            float t = 1.0f * i / (points.Count - 1);
            Gizmos.color = Color.Lerp(pointStartColor, pointFinishColor, t);
            Gizmos.DrawWireSphere(points[i].position, 0.3f);
        }

        // Pick which TrailPresenter to use
        TrailPresenter presenter = null;
        switch (this.curveType)
        {
            case TrailCurveType.LINEAR:
                presenter = new LinearPresenter();
                break;
            case TrailCurveType.CATMULL_ROM:
                presenter = new CatmullRomPresenter(loopType);
                break;
        }
        presenter.SetPoints(points);

        // Apply presenter to data set
        int lastIndex = points.Count;
		if (loopType == iTween.LoopType.loop)
        {
            lastIndex++;
        }
        Color trailStartColor = new Color(0.75f, 0f, 0f);
        Color trailFinishColor = new Color(0f, 0f, 0.75f);
        for (int i = 0; i < lastIndex - 1; i++)
        {
            float t = 1.0f * i / (points.Count - 1);
            Gizmos.color = Color.Lerp(trailStartColor, trailFinishColor, t);
            presenter.DrawTrailAt(i);
        }

    }

    public abstract class TrailPresenter
    {

        protected List<Transform> points;

        public void SetPoints(List<Transform> points)
        {
            this.points = points;
        }

        abstract public void DrawTrailAt(int position);

        //Clamp the list positions to allow looping
        //start over again when reaching the end or beginning
        protected int ClampListPos(int pos)
        {
            if (pos < 0)
            {
                pos = points.Count - 1;
            }

            if (pos > points.Count)
            {
                pos = 1;
            }
            else if (pos > points.Count - 1)
            {
                pos = 0;
            }

            return pos;
        }
    }

    public class LinearPresenter : TrailPresenter
    {

        override public void DrawTrailAt(int i)
        {
            Vector3 pStart = points[i].position;
            Vector3 pFinish = points[ClampListPos(i + 1)].position;
            Gizmos.DrawLine(pStart, pFinish);
        }
    }

    public class CatmullRomPresenter : TrailPresenter
    {
        private bool isRepeating;

        /**
		 * Class mechanics stolen from http://www.habrador.com/tutorials/catmull-rom-splines/
		 */
		public CatmullRomPresenter(iTween.LoopType loopType)
        {
			this.isRepeating = loopType == iTween.LoopType.loop;
        }

        override public void DrawTrailAt(int i)
        {
            if (!isRepeating && (i == 0 || i > points.Count - 3))
            {
                return;
            }

            //Clamp to allow looping
            Vector3 p0 = points[ClampListPos(i - 1)].position;
            Vector3 p1 = points[i].position;
            Vector3 p2 = points[ClampListPos(i + 1)].position;
            Vector3 p3 = points[ClampListPos(i + 2)].position;


            //Just assign a tmp value to this
            Vector3 lastPos = Vector3.zero;

            //t is always between 0 and 1 and determines the resolution of the spline
            //0 is always at p1
            for (float t = 0; t < 1; t += 0.1f)
            {
                //Find the coordinates between the control points with a Catmull-Rom spline
                Vector3 newPos = InterpolateCatmullRom(t, p0, p1, p2, p3);

                //Cant display anything the first iteration
                if (t == 0)
                {
                    lastPos = newPos;
                    continue;
                }

                Gizmos.DrawLine(lastPos, newPos);
                lastPos = newPos;
            }

            //Also draw the last line since it is always less than 1, so we will always miss it
            Gizmos.DrawLine(lastPos, p2);

        }

        Vector3 InterpolateCatmullRom(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            Vector3 a = 0.5f * (2f * p1);
            Vector3 b = 0.5f * (p2 - p0);
            Vector3 c = 0.5f * (2f * p0 - 5f * p1 + 4f * p2 - p3);
            Vector3 d = 0.5f * (-p0 + 3f * p1 - 3f * p2 + p3);

            Vector3 pos = a + (b * t) + (c * t * t) + (d * t * t * t);

            return pos;
        }
    }


}
