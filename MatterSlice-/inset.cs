/*
This file is part of MatterSlice. A commandline utility for
generating 3D printing GCode.

Copyright (C) 2013 David Braam
Copyright (c) 2014, Lars Brubaker

MatterSlice is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as
published by the Free Software Foundation, either version 3 of the
License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using MatterSlice.ClipperLib;
using System.Collections.Generic;

namespace MatterHackers.MatterSlice
{
	using Polygons = List<List<IntPoint>>;

	public static class Inset
	{
		public static void GenerateInsets(SliceLayerPart part, int offset_um, int outerPerimeterOffset_um, int insetCount)
		{
			double minimumDistanceToCreateNewPosition = 10;

			int currentOffset = 0;
			int offsetBy = outerPerimeterOffset_um / 2;
		
			part.AvoidCrossingBoundery = part.TotalOutline.Offset(-offset_um);
			if (insetCount == 0)
			{
				// if we have no insets defined still create one
				part.Insets.Add(part.TotalOutline);
			}
			else // generate the insets
			{
				for (int i = 0; i < insetCount; i++)
				{
					currentOffset += offsetBy;
		
					Polygons currentInset = part.TotalOutline.Offset(-currentOffset);
					// make sure our polygon data is reasonable
					currentInset = Clipper.CleanPolygons(currentInset, minimumDistanceToCreateNewPosition);

					// check that we have actuall paths
					if (currentInset.Count > 0)
					{
						part.Insets.Add(currentInset);
						currentOffset += (offsetBy + offset_um / 2);
						offsetBy = offset_um / 2;
					}
					else
					{
						// we are done making insets as we have no arrea left
						break;
					}

					currentOffset += offsetBy;

					if (i == 0)
					{
						offsetBy = offset_um / 2;
					}
				}
			}
		}

		public static void generateInsets(SliceLayer layer, int offset_um, int outerPerimeterOffset_um, int insetCount)
		{
			for (int partIndex = 0; partIndex < layer.parts.Count; partIndex++)
			{
				GenerateInsets(layer.parts[partIndex], offset_um, outerPerimeterOffset_um, insetCount);
			}

			//Remove the parts which did not generate an inset. As these parts are too small to print,
			// and later code can now assume that there is always minimum 1 inset line.
			for (int partIndex = 0; partIndex < layer.parts.Count; partIndex++)
			{
				if (layer.parts[partIndex].Insets.Count < 1)
				{
					layer.parts.RemoveAt(partIndex);
					partIndex -= 1;
				}
			}
		}
	}
}