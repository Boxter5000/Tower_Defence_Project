﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGeneration : MonoBehaviour {

	[SerializeField]
	NoiseMapGeneration noiseMapGeneration;

	[SerializeField]
	private MeshRenderer tileRenderer;

	[SerializeField]
	private MeshFilter meshFilter;

	[SerializeField]
	private MeshCollider meshCollider;

	[SerializeField]
	private float levelScale;

	[SerializeField]
	private TerrainType[] heightTerrainTypes;

	[SerializeField]
	private TerrainType[] heatTerrainTypes;

	[SerializeField]
	private TerrainType[] moistureTerrainTypes;

	[SerializeField]
	private float heightMultiplier;

	[SerializeField]
	private AnimationCurve heightCurve;

	[SerializeField]
	private AnimationCurve heatCurve;

	[SerializeField]
	private AnimationCurve moistureCurve;

	[SerializeField]
	private Wave[] heightWaves;

	[SerializeField]
	private Wave[] heatWaves;

	[SerializeField]
	private Wave[] moistureWaves;

	[SerializeField]
	private BiomeRow[] biomes;

	[SerializeField]
	private Color waterColor;

	[SerializeField]
	private VisualizationMode visualizationMode;

	public void GenerateTile(float centerVertexZ, float maxDistanceZ) {
		// calculate tile depth and width based on the mesh vertices
		Vector3[] meshVertices = this.meshFilter.mesh.vertices;
		int tileDepth = (int)Mathf.Sqrt(meshVertices.Length);
		int tileWidth = tileDepth;

		// calculate the offsets based on the tile position
		float offsetX = -this.gameObject.transform.position.x;
		float offsetZ = -this.gameObject.transform.position.z;

		// generate a heightMap using Perlin Noise
		float[,] heightMap = this.noiseMapGeneration.GeneratePerlinNoiseMap (tileDepth, tileWidth, this.levelScale, offsetX, offsetZ, this.heightWaves);

		// calculate vertex offset based on the Tile position and the distance between vertices
		Vector3 tileDimensions = this.meshFilter.mesh.bounds.size;
		float distanceBetweenVertices = tileDimensions.z / (float)tileDepth;
		float vertexOffsetZ = this.gameObject.transform.position.z / distanceBetweenVertices;

		// generate a heatMap using uniform noise
		float[,] uniformHeatMap = this.noiseMapGeneration.GenerateUniformNoiseMap (tileDepth, tileWidth, centerVertexZ, maxDistanceZ, vertexOffsetZ);
		// generate a heatMap using Perlin Noise
		float[,] randomHeatMap = this.noiseMapGeneration.GeneratePerlinNoiseMap (tileDepth, tileWidth, this.levelScale, offsetX, offsetZ, this.heatWaves);
		float[,] heatMap = new float[tileDepth, tileWidth];
		for (int zIndex = 0; zIndex < tileDepth; zIndex++) {
			for (int xIndex = 0; xIndex < tileWidth; xIndex++) {
				// mix both heat maps together by multiplying their values
				heatMap [zIndex, xIndex] = uniformHeatMap [zIndex, xIndex] * randomHeatMap [zIndex, xIndex];
				// makes higher regions colder, by adding the height value to the heat map
				heatMap [zIndex, xIndex] += this.heatCurve.Evaluate(heightMap [zIndex, xIndex]) * heightMap [zIndex, xIndex];
			}
		}

		// generate a moistureMap using Perlin Noise
		float[,] moistureMap = this.noiseMapGeneration.GeneratePerlinNoiseMap (tileDepth, tileWidth, this.levelScale, offsetX, offsetZ, this.moistureWaves);
		for (int zIndex = 0; zIndex < tileDepth; zIndex++) {
			for (int xIndex = 0; xIndex < tileWidth; xIndex++) {
				// makes higher regions dryer, by reducing the height value from the heat map
				moistureMap [zIndex, xIndex] -= this.moistureCurve.Evaluate(heightMap [zIndex, xIndex]) * heightMap [zIndex, xIndex];
			}
		}

		// build a Texture2D from the height map
		TerrainType[,] chosenHeightTerrainTypes = new TerrainType[tileDepth, tileWidth];
		Texture2D heightTexture = BuildTexture (heightMap, this.heightTerrainTypes, chosenHeightTerrainTypes);
		// build a Texture2D from the heat map
		TerrainType[,] chosenHeatTerrainTypes = new TerrainType[tileDepth, tileWidth];
		Texture2D heatTexture = BuildTexture (heatMap, this.heatTerrainTypes, chosenHeatTerrainTypes);
		// build a Texture2D from the moisture map
		TerrainType[,] chosenMoistureTerrainTypes = new TerrainType[tileDepth, tileWidth];
		Texture2D moistureTexture = BuildTexture (moistureMap, this.moistureTerrainTypes, chosenMoistureTerrainTypes);

		// build a biomes Texture2D from the three other noise variables
		Texture2D biomeTexture = BuildBiomeTexture(chosenHeightTerrainTypes, chosenHeatTerrainTypes, chosenMoistureTerrainTypes);

		switch (this.visualizationMode) {
		case VisualizationMode.Height:
			// assign material texture to be the heightTexture
			this.tileRenderer.material.mainTexture = heightTexture;
			break;
		case VisualizationMode.Heat:
			// assign material texture to be the heatTexture
			this.tileRenderer.material.mainTexture = heatTexture;
			break;
		case VisualizationMode.Moisture:
			// assign material texture to be the moistureTexture
			this.tileRenderer.material.mainTexture = moistureTexture;
			break;
		case VisualizationMode.Biome:
			// assign material texture to be the moistureTexture
			this.tileRenderer.material.mainTexture = biomeTexture;
			break;
		}

		// update the tile mesh vertices according to the height map
		UpdateMeshVertices (heightMap);
	}

	private Texture2D BuildTexture(float[,] heightMap, TerrainType[] terrainTypes, TerrainType[,] chosenTerrainTypes) {
		int tileDepth = heightMap.GetLength (0);
		int tileWidth = heightMap.GetLength (1);

		Color[] colorMap = new Color[tileDepth * tileWidth];
		for (int zIndex = 0; zIndex < tileDepth; zIndex++) {
			for (int xIndex = 0; xIndex < tileWidth; xIndex++) {
				// transform the 2D map index is an Array index
				int colorIndex = zIndex * tileWidth + xIndex;
				float height = heightMap [zIndex, xIndex];
				// choose a terrain type according to the height value
				TerrainType terrainType = ChooseTerrainType (height, terrainTypes);
				// assign the color according to the terrain type
				colorMap[colorIndex] = terrainType.color;

				// save the chosen terrain type
				chosenTerrainTypes [zIndex, xIndex] = terrainType;
			}
		}

		// create a new texture and set its pixel colors
		Texture2D tileTexture = new Texture2D (tileWidth, tileDepth);
		tileTexture.wrapMode = TextureWrapMode.Clamp;
		tileTexture.SetPixels (colorMap);
		tileTexture.Apply ();

		return tileTexture;
	}

	TerrainType ChooseTerrainType(float noise, TerrainType[] terrainTypes) {
		// for each terrain type, check if the height is lower than the one for the terrain type
		foreach (TerrainType terrainType in terrainTypes) {
			// return the first terrain type whose height is higher than the generated one
			if (noise < terrainType.threshold) {
				return terrainType;
			}
		}
		return terrainTypes [terrainTypes.Length - 1];
	}

	private void UpdateMeshVertices(float[,] heightMap) {
		int tileDepth = heightMap.GetLength (0);
		int tileWidth = heightMap.GetLength (1);

		Vector3[] meshVertices = this.meshFilter.mesh.vertices;

		// iterate through all the heightMap coordinates, updating the vertex index
		int vertexIndex = 0;
		for (int zIndex = 0; zIndex < tileDepth; zIndex++) {
			for (int xIndex = 0; xIndex < tileWidth; xIndex++) {
				float height = heightMap [zIndex, xIndex];

				Vector3 vertex = meshVertices [vertexIndex];
				// change the vertex Y coordinate, proportional to the height value. The height value is evaluated by the heightCurve function, in order to correct it.
				meshVertices[vertexIndex] = new Vector3(vertex.x, this.heightCurve.Evaluate(height) * this.heightMultiplier, vertex.z);

				vertexIndex++;
			}
		}

		// update the vertices in the mesh and update its properties
		this.meshFilter.mesh.vertices = meshVertices;
		this.meshFilter.mesh.RecalculateBounds ();
		this.meshFilter.mesh.RecalculateNormals ();
		// update the mesh collider
		this.meshCollider.sharedMesh = this.meshFilter.mesh;
	}

	private Texture2D BuildBiomeTexture(TerrainType[,] heightTerrainTypes, TerrainType[,] heatTerrainTypes, TerrainType[,] moistureTerrainTypes) {
		int tileDepth = heatTerrainTypes.GetLength (0);
		int tileWidth = heatTerrainTypes.GetLength (1);

		Color[] colorMap = new Color[tileDepth * tileWidth];
		for (int zIndex = 0; zIndex < tileDepth; zIndex++) {
			for (int xIndex = 0; xIndex < tileWidth; xIndex++) {
				int colorIndex = zIndex * tileWidth + xIndex;

				TerrainType heightTerrainType = heightTerrainTypes [zIndex, xIndex];
				// check if the current coordinate is a water region
				if (heightTerrainType.name != "water") {
					// if a coordinate is not water, its biome will be defined by the heat and moisture values
					TerrainType heatTerrainType = heatTerrainTypes [zIndex, xIndex];
					TerrainType moistureTerrainType = moistureTerrainTypes [zIndex, xIndex];

					// terrain type index is used to access the biomes table
					Biome biome = this.biomes [moistureTerrainType.index].biomes [heatTerrainType.index];
					// assign the color according to the selected biome
					colorMap [colorIndex] = biome.color;
				} else {
					// water regions don't have biomes, they always have the same color
					colorMap [colorIndex] = this.waterColor;
				}
			}
		}

		// create a new texture and set its pixel colors
		Texture2D tileTexture = new Texture2D (tileWidth, tileDepth);
		tileTexture.wrapMode = TextureWrapMode.Clamp;
		tileTexture.SetPixels (colorMap);
		tileTexture.Apply ();

		return tileTexture;
	}
}

[System.Serializable]
public class TerrainType {
	public string name;
	public float threshold;
	public Color color;
	public int index;
}

[System.Serializable]
public class Biome {
	public string name;
	public Color color;
}

[System.Serializable]
public class BiomeRow {
	public Biome[] biomes;
}

enum VisualizationMode {Height, Heat, Moisture, Biome}