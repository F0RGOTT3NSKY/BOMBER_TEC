/*!
* @file Grid.cs 
* @author Adrian Gomez Garro
* @date 10/11/2020
* @brief  Codigo que dibuja en el Gizmos el pathfinding.
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*!
 * @class Grid
 * @brief La clase Grid se encarga de gestionar la parte de debug para ver el pathfinding y establecer el camino libre en el juego.
 * @details Con esta clase se puede gestionar todo lo referente a los genes de los cromosomas de cada individio. 
 * @public
 */
public class Grid : MonoBehaviour {
	/// Para indicar si ver el grid en el Gizmos.
	public bool displayGridGizmos;
	/// Indica la layer en la que no se puede caminar.
	public LayerMask unwalkableMask;
	/// Indica el tamano del grid
    public Vector2 gridWorldSize = new Vector2(seeMap.NFilas_Map, seeMap.NColumnas_Map);
	/// indica el tamano de los nodos del grid.
	public float nodeRadius;
	/// Indica los terrenos en los que se pueden caminar.
	public TerrainType[] walkableRegions;
	/// Indica la penalizacion de caminar al lado de un obstaculo.
	public int obstacleProximityPenalty = 10;
	/// Indica el diccionario de regiones en las que se puede caminar.
	Dictionary<int,int> walkableRegionsDictionary = new Dictionary<int, int>();
	/// Indica la Layer por la que se puede caminar.
	LayerMask walkableMask;
	/// Indica cada nodo del grid.
	Node[,] grid;
	/// Indica el diametro de cada nodo en el grid.
	float nodeDiameter;
	///Indica el tamano en X del grid.
	int gridSizeX, 
	/// Indica el tamano en Y del grid.
		 gridSizeY;
	/// Indica la penalizacion maxima.
	int penaltyMin = int.MaxValue;
	/// Indica la penalizacion minima.
	int penaltyMax = int.MinValue;

	/*!
	 * @brief Awake() se llama al cargar la instancia de Grid.cs
	 * @details Se usa para establecer el tamano del grid y llamar a la funcion CreateGrid().
	 */
	public void Awake() {
		nodeDiameter = nodeRadius*2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);

		foreach (TerrainType region in walkableRegions) {
			walkableMask.value |= region.terrainMask.value;
			walkableRegionsDictionary.Add((int)Mathf.Log(region.terrainMask.value,2),region.terrainPenalty);
		}

		CreateGrid();
	}
	/*!
	 * @brief Se usa para obtener el tamano maximo.
	 */
	public int MaxSize {
		get {
			return gridSizeX * gridSizeY;
		}
	}
	/*!
	 * @brief LateUpdate() se llama en cada frame
	 * @details Se usa para actualizar el grid al existir cambios en el escenario.
	 */
    public void LateUpdate()
    {
		CreateGrid();
    }
	/*!
	 * @brief CreateGrid() se usa para detectar el escenario y asi crear el grid.
	 * @details Se utiliza un RayCastHit para detectar colisiones y asi crear el grid con los espacios libres del mapa.
	 */
    public void CreateGrid() {
		grid = new Node[gridSizeX,gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;

		for (int x = 0; x < gridSizeX; x ++) {
			for (int y = 0; y < gridSizeY; y ++) {
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint,nodeRadius,unwalkableMask));

				int movementPenalty = 0;


				Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
				RaycastHit hit;
				if (Physics.Raycast(ray,out hit, 100, walkableMask)) {
					walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
				}

				if (!walkable) {
					movementPenalty += obstacleProximityPenalty;
				}


				grid[x,y] = new Node(walkable,worldPoint, x,y, movementPenalty);
			}
		}

		BlurPenaltyMap (3);

	}
	/*!
	 * @brief BlurPenaltyMap() se utiliza para calcular cuales areas en el grid estan siendo penalizadas y son mas costosas para el pathfinding.
	 */
	void BlurPenaltyMap(int blurSize) {
		int kernelSize = blurSize * 2 + 1;
		int kernelExtents = (kernelSize - 1) / 2;

		int[,] penaltiesHorizontalPass = new int[gridSizeX,gridSizeY];
		int[,] penaltiesVerticalPass = new int[gridSizeX,gridSizeY];

		for (int y = 0; y < gridSizeY; y++) {
			for (int x = -kernelExtents; x <= kernelExtents; x++) {
				int sampleX = Mathf.Clamp (x, 0, kernelExtents);
				penaltiesHorizontalPass [0, y] += grid [sampleX, y].movementPenalty;
			}

			for (int x = 1; x < gridSizeX; x++) {
				int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, gridSizeX);
				int addIndex = Mathf.Clamp(x + kernelExtents, 0, gridSizeX-1);

				penaltiesHorizontalPass [x, y] = penaltiesHorizontalPass [x - 1, y] - grid [removeIndex, y].movementPenalty + grid [addIndex, y].movementPenalty;
			}
		}
			
		for (int x = 0; x < gridSizeX; x++) {
			for (int y = -kernelExtents; y <= kernelExtents; y++) {
				int sampleY = Mathf.Clamp (y, 0, kernelExtents);
				penaltiesVerticalPass [x, 0] += penaltiesHorizontalPass [x, sampleY];
			}

			int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass [x, 0] / (kernelSize * kernelSize));
			grid [x, 0].movementPenalty = blurredPenalty;

			for (int y = 1; y < gridSizeY; y++) {
				int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, gridSizeY);
				int addIndex = Mathf.Clamp(y + kernelExtents, 0, gridSizeY-1);

				penaltiesVerticalPass [x, y] = penaltiesVerticalPass [x, y-1] - penaltiesHorizontalPass [x,removeIndex] + penaltiesHorizontalPass [x, addIndex];
				blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass [x, y] / (kernelSize * kernelSize));
				grid [x, y].movementPenalty = blurredPenalty;

				if (blurredPenalty > penaltyMax) {
					penaltyMax = blurredPenalty;
				}
				if (blurredPenalty < penaltyMin) {
					penaltyMin = blurredPenalty;
				}
			}
		}

	}
	/*!
	 * @brief GetNeighbours() se usa para determinar los nodos cercanos al que esta usando el personaje.
	 * @param node Indica el nodo que se esta usando en ese momento.
	 * @return Retorna una lista de nodos los cuales son los nodos cercanos al nodo inicial.
	 */
	public List<Node> GetNeighbours(Node node) {
		List<Node> neighbours = new List<Node>();

		int CheckX;
		int CheckY;

		CheckX = node.gridX + 1;
		CheckY = node.gridY;
		if (CheckX >= 0 && CheckX < gridSizeX)
		{
			if (CheckY >= 0 && CheckY < gridSizeY)
			{
				neighbours.Add(grid[CheckX, CheckY]);
			}
		}

		CheckX = node.gridX - 1;
		CheckY = node.gridY;
		if (CheckX >= 0 && CheckX < gridSizeX)
		{
			if (CheckY >= 0 && CheckY < gridSizeY)
			{
				neighbours.Add(grid[CheckX, CheckY]);
			}
		}

		CheckX = node.gridX;
		CheckY = node.gridY + 1;
		if (CheckX >= 0 && CheckX < gridSizeX)
		{
			if (CheckY >= 0 && CheckY < gridSizeY)
			{
				neighbours.Add(grid[CheckX, CheckY]);
			}
		}
		CheckX = node.gridX;
		CheckY = node.gridY - 1;
		if (CheckX >= 0 && CheckX < gridSizeX)
		{
			if (CheckY >= 0 && CheckY < gridSizeY)
			{
				neighbours.Add(grid[CheckX, CheckY]);
			}
		}
		return neighbours;
	}

	/*!
	 * @brief NodeFromWorldPoint() se usa para obtener un nodo en un punto especifico del escenario.
	 * @param worldPosition Indica la posicion en el mapa.
	 * @return Retorna la posicion en el grid del nodo.
	 */
	public Node NodeFromWorldPoint(Vector3 worldPosition) {
		float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percentY = (worldPosition.z + gridWorldSize.y/2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
		return grid[x,y];
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x,1,gridWorldSize.y));
		if (grid != null && displayGridGizmos) {
			foreach (Node n in grid) {

				Gizmos.color = Color.Lerp (Color.white, Color.black, Mathf.InverseLerp (penaltyMin, penaltyMax, n.movementPenalty));
				Gizmos.color = (n.walkable)?Gizmos.color:Color.red;
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter));
			}
		}
	}

	[System.Serializable]
	public class TerrainType {
		public LayerMask terrainMask;
		public int terrainPenalty;
	}


}