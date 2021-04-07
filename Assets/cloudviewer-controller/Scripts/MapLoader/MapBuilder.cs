using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AutoCore.OSM;

public class MapBuilder : MonoBehaviour
{
    public Dictionary<int, MapElement> MapDictionary=new Dictionary<int, MapElement>();
    OSMData data;
    public OSMReader reader;
    public GameObject line_thin;
    public GameObject stop_line;
    public GameObject traffic_light;
    public GameObject traffic_sign;
    public Transform linethinParent;
    public Transform stoplineParent;
    public Transform trafficlightParent;
    public Transform trafficsignParent;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ClearMap()
    {
        MapDictionary = new Dictionary<int, MapElement>();
        while (linethinParent.childCount != 0)
        {
            DestroyImmediate(linethinParent.GetChild(0).gameObject);
        }
        while (stoplineParent.childCount != 0)
        {
            DestroyImmediate(stoplineParent.GetChild(0).gameObject);
        }
        while (trafficlightParent.childCount != 0)
        {
            DestroyImmediate(trafficlightParent.GetChild(0).gameObject);
        }
        while (trafficsignParent.childCount != 0)
        {
            DestroyImmediate(trafficsignParent.GetChild(0).gameObject);
        }
    }
    public void BuildMap()
    {
        ClearMap();
        if (reader==null) reader = GetComponent<OSMReader>();
        if (data == null) data = reader.GetOSMData(); 
        foreach (Way way in data.ways)
        {
            GameObject go;
            Vector3[] points = new Vector3[way.nodes.Count];
            for (int i = 0; i < way.nodes.Count; i++)
            {
                points[i] = data.nodes[way.nodes[i]].GetPosition();
            }
            switch (way.type)
            {
                case WayType.line_thin:
                    go = Instantiate(line_thin, linethinParent);
                    go.GetComponent<Line_thin>().UpdateLineRenderer(points);
                    go.GetComponent<Line_thin>().SetElementID(way.id);
                    MapDictionary.Add(way.id, go.GetComponent<MapElement>());
                    break;
                case WayType.stop_line:
                    go = Instantiate(stop_line, stoplineParent);
                    go.GetComponent<Stop_line>().UpdateLineRenderer(points);
                    go.GetComponent<Stop_line>().SetElementID(way.id);
                    MapDictionary.Add(way.id, go.GetComponent<MapElement>());
                    break;
                case WayType.traffic_light:
                    Vector3 trafficPos = (points[0] + points[1])/2;
                    go = Instantiate(traffic_light, trafficlightParent);
                    go.transform.position=trafficPos;
                    go.GetComponent<Traffic_light>().SetElementID(way.id);
                    MapDictionary.Add(way.id, go.GetComponent<MapElement>());
                    break;
                case WayType.traffic_sign:
                    go = Instantiate(traffic_sign, trafficsignParent);
                    go.GetComponent<Traffic_sign>().UpdateLineRenderer(points);
                    go.GetComponent<Traffic_sign>().SetElementID(way.id);
                    MapDictionary.Add(way.id, go.GetComponent<MapElement>());
                    break;
                default:
                    break;
            }
            
        }

        foreach (Relation relation in data.relations)
        {
            if (relation.subType == RelationSubType.traffic_light)
            {
                Vector3 targetPos=new Vector3();
                Traffic_light light = new Traffic_light();
                foreach (Member member in relation.menbers)
                {
                    if (MapDictionary.TryGetValue(member.refID, out MapElement element))
                    {
                        if (member.roleType == RoleType.ref_line)
                        {
                            Line line = element.GetComponent<Line>();
                            targetPos = line.lineRenderer.GetPosition(0) + line.lineRenderer.GetPosition(1);
                            targetPos *= 0.5f;
                        }
                        else if (member.roleType == RoleType.refers)
                        {
                            light = element.GetComponent<Traffic_light>();
                        }
                    }
                }
                targetPos.y = light.transform.position.y;
                light.transform.LookAt(targetPos);
            }
        }

    }
}
