using MatterSlice.ClipperLib;
using System.Collections.Generic;

namespace MatterHackers.MatterSlice
{
	using Polygons = List<List<IntPoint>>;

	public class Skirt
	{
		public static void generateSkirt(SliceDataStorage storage, int distance, int extrusionWidth_um, int numberOfLoops, int minLength, int initialLayerHeight, ConfigSettings config)
		{
			bool externalOnly = (distance > 0);
			for (int skirtLoop = 0; skirtLoop < numberOfLoops; skirtLoop++)
			{
				int offsetDistance = distance + extrusionWidth_um * skirtLoop + extrusionWidth_um / 2;

				Polygons skirtPolygons = new Polygons(storage.wipeTower.Offset(offsetDistance));
				for (int volumeIndex = 0; volumeIndex < storage.volumes.Count; volumeIndex++)
				{
					if (config.continuousSpiralOuterPerimeter && volumeIndex > 0)
					{
						continue;
					}

					if (storage.volumes[volumeIndex].layers.Count < 1)
					{
						continue;
					}

					SliceLayer layer = storage.volumes[volumeIndex].layers[0];
					for (int partIndex = 0; partIndex < layer.parts.Count; partIndex++)
					{
						if (config.continuousSpiralOuterPerimeter && partIndex > 0)
						{
							continue;
						}

						if (externalOnly)
						{
							Polygons p = new Polygons();
							p.Add(layer.parts[partIndex].TotalOutline[0]);
							//p.Add(IntPointHelper.CreateConvexHull(layer.parts[partIndex].outline[0]));
							skirtPolygons = skirtPolygons.CreateUnion(p.Offset(offsetDistance));
						}
						else
						{
							skirtPolygons = skirtPolygons.CreateUnion(layer.parts[partIndex].TotalOutline.Offset(offsetDistance));
						}
					}
				}

				SupportPolyGenerator supportGenerator = new SupportPolyGenerator(storage.support, initialLayerHeight);
				skirtPolygons = skirtPolygons.CreateUnion(supportGenerator.supportPolygons.Offset(offsetDistance));

				//Remove small inner skirt holes. Holes have a negative area, remove anything smaller then 100x extrusion "area"
				for (int n = 0; n < skirtPolygons.Count; n++)
				{
					double area = skirtPolygons[n].Area();
					if (area < 0 && area > -extrusionWidth_um * extrusionWidth_um * 100)
					{
						skirtPolygons.RemoveAt(n--);
					}
				}

				storage.skirt.AddAll(skirtPolygons);

				int lenght = (int)storage.skirt.PolygonLength();
				if (skirtLoop + 1 >= numberOfLoops && lenght > 0 && lenght < minLength)
				{
					// add more loops for as long as we have not extruded enough length
					numberOfLoops++;
				}
			}
		}
	}
}