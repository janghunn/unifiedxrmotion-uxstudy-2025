using UnityEngine;
using System.Collections.Generic;

// Attach this to any GameObject and set `source` in Inspector
public class ExtractHandsFromSkinnedMesh : MonoBehaviour
{
    public SkinnedMeshRenderer source;
    [Range(0f, 1f)] public float minWeight = 0.001f;   // Keep if weight > threshold
    public bool requireAllVertices = true;             // True: all 3 verts must be "hand-influenced"
    public string[] nameContains =                    // Bone name filters (edit to fit your rig)
        { "Hand", "Thumb", "Index", "Middle", "Ring", "Little", "Pinky", "Finger" };

    [ContextMenu("Extract Hands")]
    public void ExtractHands()
    {
        if (!source || !source.sharedMesh) { Debug.LogWarning("No source mesh."); return; }

        var mesh = source.sharedMesh;
        var bones = source.bones;
        var bindposes = mesh.bindposes;

        // 1) Build allowed bone index set by name
        var allowed = new HashSet<int>();
        for (int i = 0; i < bones.Length; i++)
        {
            string n = bones[i] ? bones[i].name : "";
            for (int k = 0; k < nameContains.Length; k++)
                if (n.IndexOf(nameContains[k], System.StringComparison.OrdinalIgnoreCase) >= 0)
                { allowed.Add(i); break; }
        }
        if (allowed.Count == 0) { Debug.LogWarning("No hand-like bones found."); return; }

        // 2) Mark vertices influenced by allowed bones
        var bw = mesh.boneWeights; // OK for most rigs; uses 4 weights per vertex
        var keepV = new bool[mesh.vertexCount];
        for (int v = 0; v < bw.Length; v++)
        {
            var b = bw[v];
            bool keep =
                (b.weight0 > minWeight && allowed.Contains(b.boneIndex0)) ||
                (b.weight1 > minWeight && allowed.Contains(b.boneIndex1)) ||
                (b.weight2 > minWeight && allowed.Contains(b.boneIndex2)) ||
                (b.weight3 > minWeight && allowed.Contains(b.boneIndex3));
            keepV[v] = keep;
        }

        // 3) Prepare attribute copies
        var srcVerts = mesh.vertices;
        var srcNorms = mesh.normals;
        var srcTans  = mesh.tangents;
        var srcUV0   = mesh.uv;

        var newVerts = new List<Vector3>();
        var newNorms = new List<Vector3>();
        var newTans  = new List<Vector4>();
        var newUV0   = new List<Vector2>();
        var newBW    = new List<BoneWeight>();
        var remap    = new Dictionary<int, int>(mesh.vertexCount);

        int subCount = mesh.subMeshCount;
        var newTrisPerSub = new List<List<int>>(subCount);
        for (int s = 0; s < subCount; s++) newTrisPerSub.Add(new List<int>());

        // 4) Copy triangles if they belong to hand area
        for (int s = 0; s < subCount; s++)
        {
            var tris = mesh.GetTriangles(s);
            var outTris = newTrisPerSub[s];
            for (int t = 0; t < tris.Length; t += 3)
            {
                int a = tris[t], b2 = tris[t + 1], c = tris[t + 2];
                int keepCount = (keepV[a] ? 1 : 0) + (keepV[b2] ? 1 : 0) + (keepV[c] ? 1 : 0);
                bool take = requireAllVertices ? (keepCount == 3) : (keepCount >= 2);
                if (!take) continue;

                int na = Remap(a), nb = Remap(b2), nc = Remap(c);
                outTris.Add(na); outTris.Add(nb); outTris.Add(nc);
            }
        }

        // 5) Build new mesh
        int totalIdx = 0; foreach (var l in newTrisPerSub) totalIdx += l.Count;
        if (totalIdx == 0) { Debug.LogWarning("No triangles matched hand bones."); return; }

        var newMesh = new Mesh { name = mesh.name + "_HandsOnly" };
        newMesh.vertices = newVerts.ToArray();
        if (srcNorms != null && srcNorms.Length == mesh.vertexCount) newMesh.normals = newNorms.ToArray();
        if (srcTans  != null && srcTans.Length  == mesh.vertexCount) newMesh.tangents = newTans.ToArray();
        if (srcUV0   != null && srcUV0.Length   == mesh.vertexCount) newMesh.uv = newUV0.ToArray();
        newMesh.boneWeights = newBW.ToArray();
        newMesh.bindposes = bindposes;
        newMesh.subMeshCount = subCount;
        for (int s = 0; s < subCount; s++) newMesh.SetTriangles(newTrisPerSub[s], s, true);
        newMesh.RecalculateBounds();

        // 6) Spawn a new SMR with the extracted mesh
        var go = new GameObject(source.name + "_HandsOnly");
        go.transform.SetParent(source.transform, false);
        var dst = go.AddComponent<SkinnedMeshRenderer>();
        dst.sharedMesh = newMesh;
        dst.bones = source.bones;      // Keep original bone array (indices stay valid)
        dst.rootBone = source.rootBone;
        dst.updateWhenOffscreen = source.updateWhenOffscreen;

        // Copy materials (only subs with triangles will render)
        var srcMats = source.sharedMaterials;
        var newMats = new Material[Mathf.Min(srcMats.Length, subCount)];
        for (int s = 0; s < newMats.Length; s++)
            newMats[s] = (newTrisPerSub[s].Count > 0) ? srcMats[s] : srcMats[0];
        dst.sharedMaterials = newMats;

        // 7) Optionally hide original body
        source.enabled = false;

        // --- local remap helper ---
        int Remap(int oldIndex)
        {
            if (remap.TryGetValue(oldIndex, out int mapped)) return mapped;
            int ni = newVerts.Count;
            remap.Add(oldIndex, ni);
            newVerts.Add(srcVerts[oldIndex]);
            if (srcNorms != null && srcNorms.Length == mesh.vertexCount) newNorms.Add(srcNorms[oldIndex]);
            if (srcTans  != null && srcTans.Length  == mesh.vertexCount) newTans.Add(srcTans[oldIndex]);
            if (srcUV0   != null && srcUV0.Length   == mesh.vertexCount) newUV0.Add(srcUV0[oldIndex]);
            newBW.Add(bw[oldIndex]); // Keep original bone indices; unused bones are fine
            return ni;
        }
    }
}
