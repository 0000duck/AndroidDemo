
using MatterSlice.ClipperLib;
using System.Collections.Generic;

namespace MatterHackers.MatterSlice
{
    //The GCodePathConfig is the configuration for moves/extrusion actions. This defines at which width the line is printed and at which speed.
    public class GCodePathConfig
	{
		public bool closedLoop = true;
		public int lineWidth;
		public string gcodeComment;
		public double speed;
		public bool spiralize;

		public GCodePathConfig()
		{
		}

		public GCodePathConfig(double speed, int lineWidth, string name)
		{
			this.speed = speed;
			this.lineWidth = lineWidth;
			this.gcodeComment = name;
		}

		/// <summary>
		/// Set the data for a path cofig. This is used to define how different parts (infill, perimeters) are written to gcode.
		/// </summary>
		/// <param name="speed"></param>
		/// <param name="lineWidth"></param>
		/// <param name="gcodeComment"></param>
		/// <param name="closedLoop"></param>
		public void SetData(double speed, int lineWidth, string gcodeComment, bool closedLoop = true)
		{
			this.closedLoop = closedLoop;
			this.speed = speed;
			this.lineWidth = lineWidth;
			this.gcodeComment = gcodeComment;
		}
	}
}