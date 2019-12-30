using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagnumFoudation;

/*
public class s_nodegraph : MonoBehaviour {

    public s_node[,] nodeGraph;
    public s_node[] nodeGraph2;
    const float tileSize = 20f;
    public int xsi = 75, ysi = 75;
    o_plcharacter player;

    public List<s_node> path = new List<s_node>();
    void Start () {
        player = GameObject.Find("Player").GetComponent<o_plcharacter>();
        CreateNodeGraph();
	}

    void ResetNodes()
    {
        for (int x = 0; x < xsi; x++)
        {
            for (int y = 0; y < ysi; y++)
            {
                nodeGraph[x, y].h_cost = 0;
                nodeGraph[x, y].g_cost = 0;
                nodeGraph[x, y].parent = null;
            }
        }
    }
    public float CheckYFall(s_node node)
    {
        s_node nextnode = node;
        Vector2Int v = PosToVec(node.realPosition);
        int n = 0;
        while (nextnode != null)
        {
            n++;
            print(v.x + ((v.y - n) * xsi));
            if (v.x + ((v.y + n) * xsi) > nodeGraph2.Length ||
                v.x + ((v.y + n) * xsi) < 0)
                break;
            nextnode = nodeGraph2[v.x + ((v.y - n) * xsi)];
            if (nextnode == null ||
                (COLLISION_T)nextnode.COLTYPE != COLLISION_T.FALLING)
                break;
        }
        print((v.y + n) * 20);
        return (v.y - n) * 20;
    }

    public void SetPos(Vector2 v)
    {
        SetPos((int)v.x, (int)v.y);
    }
    public void SetPos(int xs, int ys)
    {
        xsi = xs;
        ysi = ys;
    }

    public s_node[] CreateNodeArray(s_map.s_tileobj[] ti)
    {
        s_node[] arr = new s_node[xsi * ysi];
        //o_collidableobject[] col = GameObject.Find("Tiles").GetComponentsInChildren<o_collidableobject>();

        int tilesize = 20;
        int x = 0, y = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            x++;
            if (x == xsi)
            {
                y++;
                x = 0;
            }
            arr[i] = new s_node();
            arr[i].realPosition = new Vector2(x * tileSize, y * tileSize);
            arr[i].COLTYPE = (int)COLLISION_T.NONE;
        }
        foreach (s_map.s_tileobj c in ti)
        {
            Vector2Int v = PosToVec(c.pos_x, c.pos_y);

            if (arr.Length < v.x + (v.y * xsi))
                continue;
            if (0 > v.x + (v.y * xsi))
                continue;

            arr[v.x + (v.y * xsi)].type = c.TYPENAME;
            arr[v.x + (v.y * xsi)].realPosition = new Vector2(c.pos_x, c.pos_y);
            arr[v.x + (v.y * xsi)].COLTYPE = c.enumthing;
            arr[v.x + (v.y * xsi)].characterExclusive = c.exceptionChar;
            
            if (arr[v.x + (v.y * xsi)].type == "teleport_object")
            {
                arr[v.x + (v.y * xsi)].COLTYPE = (int)COLLISION_T.NONE;
                arr[v.x + (v.y * xsi)].walkable = true;
            }
            arr[v.x + (v.y * xsi)].walkable = true;

            //arr[x + (y * xsi)].walkable = true;
            if (new Vector2(c.pos_x, c.pos_y) == new Vector2(x * tilesize, y * tilesize))
            {
            }
        }

        return arr;
    }

    struct s_quadNode
    {
        public s_quadNode(Vector2 topR, Vector2 downR, Vector2 topL, Vector2 downL)
        {
            points = new Vector2[4]
            {
                topL,
                topR,
                downL,
                downR
            };
        }
        Vector2[] points;
    }

    public void NodeToPolygon()
    {
        List<s_quadNode> quads = new List<s_quadNode>();

        Vector2[] points = 
            new Vector2[4] {
                new Vector2(tileSize ,0),
                new Vector2(tileSize, tileSize),
                new Vector2(0,tileSize),
                new Vector2(0,0),
            };

        bool[,] nodebool = new bool[(xsi + 1), (ysi + 1)];

        s_node node = null;
        for (int x = 0; x < xsi; x++)
        {
            for (int y = ysi; y > 0 ; y--)
            {
                print(nodebool[x, y]);
                if (nodebool[x,y] == true)
                    continue;

                node = PosToNode(new Vector2(x * tileSize, y * tileSize));
                if (node != null)
                {
                    if ((COLLISION_T)node.COLTYPE != COLLISION_T.NONE)
                    {
                        int stoppper = 0;
                        int maxX = 0;

                        Vector2Int counter = new Vector2Int(0, 0);
                        s_node nextnode = PosToNode(new Vector2((x + counter.x) * tileSize, (y - counter.y) * tileSize));
                        while (nextnode != null || stoppper < 20)
                        {
                            nextnode = PosToNode(new Vector2((x + counter.x) * tileSize, (y - counter.y) * tileSize));
                            s_node nnnode = PosToNode(new Vector2((x + counter.x + 1) * tileSize, (y - counter.y) * tileSize));
                            if (counter.x > xsi || nnnode != null)
                            {
                                if ((COLLISION_T)nnnode.COLTYPE == COLLISION_T.NONE)
                                {
                                    if (counter.x < maxX)
                                    {
                                        maxX = counter.x;
                                    }
                                    counter.x = 0;
                                    counter.y += 1;
                                    s_node ynod = PosToNode(new Vector2(x * tileSize, (y - counter.y) * tileSize));
                                    if (ynod != null)
                                    {
                                        if ((COLLISION_T)ynod.COLTYPE == COLLISION_T.NONE)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            counter.x += 1;
                            maxX += 1;
                            stoppper++;
                        }

                        points = new Vector2[4]
                        {
                            new Vector2(maxX * tileSize ,0),
                            new Vector2(maxX* tileSize,counter.y* tileSize),
                            new Vector2(0,counter.y* tileSize),
                            new Vector2(0,0),
                        };
                        GameObject polyObj = Instantiate(new GameObject(), transform.position + new Vector3(x * tileSize, y* tileSize) + new Vector3(200,200), Quaternion.identity);
                        PolygonCollider2D poly = polyObj.AddComponent<PolygonCollider2D>();
                        poly.pathCount = 1;
                        poly.SetPath(0, points);

                        for (int x2 = x; x2 > x + maxX; x2++)
                        {
                            for (int y2 = y + counter.y; y2 < y; y2--)
                            {
                                nodebool[x2,y2] = true;
                            }
                        }

                    }
                }
            }
        }

    }
    
    public void CreateNodeGraph()
    {
        s_leveledit ed = GameObject.Find("General").GetComponent<s_leveledit>();
        GameObject mapn = ed.SceneLevelObject;
        o_generic[] colInMap = null;
        colInMap = mapn.transform.Find("Tiles").GetComponentsInChildren<o_generic>();

        nodeGraph = new s_node[xsi, ysi];
        for (int x = 0; x < xsi; x++)
        {
            for (int y = 0; y < ysi; y++)
            {
                nodeGraph[x, y] = new s_node();
                nodeGraph[x, y].realPosition = new Vector2(tileSize * x, tileSize * y);
                nodeGraph[x, y].walkable = true;
                foreach (o_generic h in colInMap)
                {
                    o_generic c = h.GetComponent<o_generic>();
                    if (c)
                    {
                        if (c.transform.position != (Vector3)new Vector2(tileSize * x, tileSize * y))
                            continue;
                        nodeGraph[x, y].COLTYPE = (int)c.collision_type;
                    }
                    else
                    {
                        nodeGraph[x, y].walkable = true;
                    }
                }
            }
        }
    }

    public Vector2Int PosToVec(int x_p, int y_p)
    {
        int x = (int)(x_p / tileSize);
        int y = (int)(y_p / tileSize);
        //print("x: " + x + " y: " + y);

        return new Vector2Int(x, y);
    }
    public Vector2Int PosToVec(Vector3 vec)
    {
        int x = (int)(vec.x / tileSize);
        int y = (int)(vec.y / tileSize);
        //print("x: " + x + " y: " + y);

        return new Vector2Int(x, y);
    }
    public s_node PosToNode(Vector2 vec)
    {
        int x = (int)(vec.x / tileSize);
        int y = (int)(vec.y / tileSize);
        if ((x + (y * xsi)) < 0 ||( x + (y * xsi)) > nodeGraph2.Length - 1)
        {
            print("Sorry! x: " + x + " y: " + y);
            return null;
        }
        return nodeGraph2[x + (y * xsi)];
    }

    public void PathFind(s_node goal, s_node start)
    {
        List<s_node> OpenList = new List<s_node>();
        HashSet<s_node> ClosedList = new HashSet<s_node>();

        if (!start.walkable || !goal.walkable)
            return;
        OpenList.Add(start);
        path = new List<s_node>();
        
        start.h_cost = Mathf.RoundToInt(HerusticVal(start.realPosition, goal.realPosition));

        while (OpenList.Count > 0) {

            s_node currentNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++) {
                s_node cur = OpenList[i];
                if (cur.f_cost < currentNode.f_cost && cur.h_cost < currentNode.h_cost) {
                        currentNode = cur;
                }
            }
            OpenList.Remove(currentNode);
            ClosedList.Add(currentNode);

            if (currentNode == goal)
            {
                path = RetracePath(goal, start);
                ResetNodes();
                print("Gocha");
                return;
            }

            Vector2Int nodepos = PosToVec(currentNode.realPosition);
            List<s_node> neighbours = new List<s_node>();
            for (int x = nodepos.x - 1; x < nodepos.x + 1; x++)
            {
                for (int y = nodepos.y - 1; y < nodepos.y + 1; y++)
                {
                    if (x > xsi || y > ysi || y < 0 || x < 0)
                        continue;
                    
                    if (y == nodepos.y - 1 && x == nodepos.x + 1 ||
                        y == nodepos.y + 1 && x == nodepos.x + 1 ||
                        y == nodepos.y + 1 && x == nodepos.x - 1 ||
                        y == nodepos.y - 1 && x == nodepos.x - 1)
                        continue;
                    
                    s_node targnod = nodeGraph[x, y];
                    
                    neighbours.Add(targnod);
                }
            }
            
            foreach (s_node no in neighbours)
            {
                if (!no.walkable || ClosedList.Contains(no))
                    continue;

                path.Add(no);
                int movcost = currentNode.g_cost + Mathf.RoundToInt(HerusticVal(currentNode.realPosition, no.realPosition)) ;
                if (movcost < no.g_cost ||
                    !OpenList.Contains(no))
                {
                    no.g_cost = movcost;
                    no.h_cost = Mathf.RoundToInt(HerusticVal(no.realPosition, goal.realPosition));
                    no.parent = currentNode;

                    if (!OpenList.Contains(no))
                        OpenList.Add(no);
                }
            }

        }
        print("OOF");
        ResetNodes();
    }
    float HerusticVal(Vector2 a, Vector2 b)
    {
        float distx = Mathf.Abs(a.x - b.x);
        float disty = Mathf.Abs(a.y - b.y);

        
        return Vector2.Distance(a, b) / tileSize;
        //D * (distx + dist)

    }
    public List<s_node> RetracePath(s_node goal, s_node start)
    {
        int i = 0;
        List<s_node> route = new List<s_node>();
        s_node current = goal;
        while (current != start)
        {
            route.Add(current);
            current = current.parent;
            i++;
            if (i == int.MaxValue)
                return route;
        }
        route.Reverse();
        return route;
    }

    private void OnDrawGizmos()
    {
        if (nodeGraph2 != null)
        {
            for (int x = 0; x < xsi; x++)
            {
                for (int y = 0; y < ysi; y++)
                {
                    s_node nod = nodeGraph2[x + (y * xsi)];
                    if (nod != null)
                    {
                        switch ((COLLISION_T)nod.COLTYPE)
                        {
                            default:

                                Gizmos.color = Color.white;
                                Gizmos.DrawWireCube(new Vector2(x * tileSize, y * tileSize), new Vector2(tileSize, tileSize));
                                break;

                            case COLLISION_T.DITCH:

                                Gizmos.color = Color.cyan;
                                Gizmos.DrawWireCube(new Vector2(x * tileSize, y * tileSize), new Vector2(tileSize, tileSize));
                                break;
                            case COLLISION_T.WALL:

                                Gizmos.color = Color.magenta;
                                Gizmos.DrawWireCube(new Vector2(x * tileSize, y * tileSize), new Vector2(tileSize, tileSize));
                                break;
                        }
                        if (player != null)
                        {
                            if (nod == PosToNode(player.transform.position + player.offsetCOL))
                            {
                                Gizmos.color = Color.red;
                                Gizmos.DrawWireCube(new Vector2(x * tileSize, y * tileSize), new Vector2(tileSize, tileSize));
                            }
                        }
                    }
                    if (path.Find(n => n == nodeGraph2[x + (y * xsi)]) != null)
                    {
                        Gizmos.color = Color.cyan;
                        Gizmos.DrawCube(new Vector2(x * tileSize, y * tileSize), new Vector2(tileSize, tileSize));
                    }
                    Vector2Int vec = PosToVec(GameObject.Find("Player").transform.position);
                    if (vec != new Vector2Int(0,0))
                        Gizmos.DrawCube(new Vector2(vec.x * tileSize, vec.y * tileSize), new Vector2(tileSize, tileSize));
                    else
                        Gizmos.color = Color.white;
                    
                    if (nodeGraph[x, y].walkable)
                    {
                        Gizmos.DrawWireCube(new Vector2(x * tileSize, y * tileSize), new Vector2(tileSize, tileSize));
                    }
                }
            }
        }
    }
}
*/
