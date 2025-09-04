# Participant Manual — UnifiedXRMotion vs Meta SDK

**Goal:** Follow clear steps to set up the same scene in Unity using **two different SDKs** and verify the results with the simulator.

* **UnifiedXRMotion** = our research SDK.
* **Meta SDK** = Meta Core SDK + Meta XR Interaction SDK (with Movement SDK for full‑body retargeting).

> Please **follow each step exactly as written**. Do **not** skip or optimize. If a step is unclear, flag the issue to the administrator.

---

## Quick Checklist (Before You Start)

* [ ] Unity installed: **6000.0.33f1**
* [ ] Project opens without errors
* [ ] Meta XR Simulator available and enabled when needed (**77.0.0**)
* [ ] Movement SDK available for Meta condition (**78.0.0**)
* [ ] You can see these assets in the **Project** window:

  * **Assets/Scenes/Task-A.unity** and **Assets/Scenes/Task-B.unity**
  * **SceneObjects**, **Y Bot**, **Hands**
  * **Hands** contains **OpenXRCustomHandPrefab\_L** and **OpenXRCustomHandPrefab\_R**

---

## Plain‑Language Glossary (1‑minute read)

* **SDK** (Software Development Kit): A set of tools that adds features to Unity.
* **Prefab**: A ready‑made object you can place into the scene (like a template).
* **Hierarchy**: The list of objects in the current scene (left side of Unity by default).
* **Inspector**: The panel to change settings of the selected object (right side of Unity by default).
* **Project window**: The file browser for assets in your project (bottom of Unity by default).
* **Simulator**: A tool that fakes body and hand movements so you can test without a headset.

You will interact with: **Hierarchy**, **Inspector**, **Project window**, and the **Simulator**.

---

## Start Here — Open Your Assigned Scene

1. In Unity, open the **Project** window.
2. Go to **Assets/Scenes/**.
3. Open both scenes once: **Task-A.unity** and **Task-B.unity**.
4. Double‑click **your assigned scene** (Task‑A or Task‑B) to make it active.

> The scene includes a basic layout: `SceneObjects`, **Y Bot**, and **Hands** with **OpenXRCustomHandPrefab\_L** and **OpenXRCustomHandPrefab\_R**.

---

## How You Will Check Your Work (both SDKs)

You will compare what you see to short reference clips.

### Run the Simulator

1. In the Unity toolbar, turn on the **Meta Simulator** toggle (next to **Play**).
2. Click **Play**.
3. In the running scene, open **Inputs → Movement Tracking Controls** in the **Meta XR Simulator** window.
4. To test **full body**: click **Play random movement**.
5. To test **hands**: use **WASD** to move hands, or **click** inside the Simulator window to make a **pinch** gesture.

### What “Correct” Looks Like

**UnifiedXRMotion — expected behavior**

<table>
  <tr>
    <td align="center" width="50%">
      <img src="Data/Videos/ours-fullbody-result.gif" width="100%" alt="FullBody Tracking (Ours)">
      <div><em>Full‑Body Tracking (UnifiedXRMotion)</em></div>
    </td>
    <td align="center" width="50%">
      <img src="Data/Videos/ours-hands-result.gif" width="100%" alt="Hands Tracking and Retargeting (Ours)">
      <div><em>Hands Tracking & Retargeting (UnifiedXRMotion)</em></div>
    </td>
  </tr>
</table>

**Meta SDK — expected behavior**

<table>
  <tr>
    <td align="center" width="50%">
      <img src="Data/Videos/meta-fullbody-result.gif" width="100%" alt="FullBody Tracking (Meta)">
      <div><em>Full‑Body Tracking (Meta)</em></div>
    </td>
    <td align="center" width="50%">
      <img src="Data/Videos/meta-hands-result.gif" width="100%" alt="Hands Tracking and Retargeting (Meta)">
      <div><em>Hands Tracking & Retargeting (Meta)</em></div>
    </td>
  </tr>
</table>

If your scene behaves like the clips, your setup is **correct**.

---

## Task‑A — UnifiedXRMotion (Our SDK)

**Goal:**

* Make the custom hand models (**OpenXRCustomHandPrefab\_L/R**) follow simulated hand motion.
* Make **Y Bot** move with simulated full‑body motion.

### Steps — Hands

1. In the **Hierarchy**, expand **Hands** and confirm **OpenXRCustomHandPrefab\_L** and **OpenXRCustomHandPrefab\_R** exist.
2. Select **Hands**. In the **Inspector**, click **Add Component** → add **Motion Avatar**.
3. In **Motion Avatar**, set **Body Type = Two Hands**.
4. In the **Hierarchy**, set up the motion systems:

   * Create **MotionSystem** (Empty object) if missing → add **Motion System** (component).
   * Under **MotionSystem**, create **TrackingSystem** → add **Tracking Manager** and **Meta Hand Tracking** (components).
   * Also under **MotionSystem**, create **RetargetSystem** → add **Retarget System** and **Two Hands Retargeter** (components).
5. Connect the parts (verify each item):

   * On **MotionSystem / Motion System**, add **TrackingSystem** to **Motion Triggers**.
   * On **RetargetSystem / Retarget System**, set **Motion Avatar = Hands**.
   * In **Retarget System**, add **Two Hands Retargeter** to **Input Motions**.
   * On **Two Hands Retargeter**, set **Input Motion = TrackingSystem**.

### Steps — Full Body

1. In the **Hierarchy**, select **Y Bot**.
2. In the **Inspector**, click **Add Component** → add **Motion Avatar**.
3. Ensure a **MotionSystem** exists (see Hands steps above). If not, create it with child objects:

   * **TrackingSystem** with **Tracking Manager** and **Meta Body Tracking**.
   * **RetargetSystem** with **Retarget System** and **Meta Body Retargeter**.
4. Connect the parts (verify each item):

   * On **MotionSystem / Motion System**, add **TrackingSystem** to **Motion Triggers**.
   * On **RetargetSystem / Retarget System**, set **Motion Avatar = Y Bot**.
   * In **Retarget System**, add **Meta Body Retargeter** to **Input Motions**.
   * On **Meta Body Retargeter**, set **Input Motion = TrackingSystem**.

### Verify — UnifiedXRMotion

1. Turn on the **Meta Simulator** toggle → click **Play**.
2. Hands: move with **WASD** or click for **pinch**.
3. Full body: **Inputs → Movement Tracking Controls → Play random movement**.
4. Compare with the **UnifiedXRMotion** reference clips above.

---

## Task‑B — Meta SDK (Meta Core + Interaction + Movement SDK)

**Goal:**

* Make **Y Bot** move with simulated full‑body motion.
* Use the provided custom hand models (**OpenXRCustomHandPrefab\_L/R**) for hands‑only motion.

### Steps — Hands

1. **Add the camera rig**

   * **Project** → **Packages/Meta XR Core SDK/Prefabs** → drag **OVRCameraRig** into the **Hierarchy**.
2. **Add hand interaction prefabs**

   * **Project** → **Packages/Meta XR Interaction SDK/Runtime/Prefabs** → drag **OVRInteractionComprehensive** under **OVRCameraRig**.
   * Keep **OVRHands** under **OVRInteractionComprehensive** (you may remove other extras).
   * On **OVR Interaction Comprehensive → OVR Camera Rig Ref**, set **Ovr Camera Rig = OVRCameraRig**.
3. **Add synthetic hands for simulation**

   * **Project** → **Packages/Meta XR Interaction SDK Essentials/Runtime/Prefabs/Hands/** → add **OVRLeftHandSynthetic** and **OVRRightHandSynthetic** under **OVRCameraRig/OVRInteractionComprehensive**.
4. **Remove default hand visuals**

   * Under **OVRLeftHandSynthetic/OVRLeftHandVisual**, delete **OpenXRLeftHand** and **OculusHand\_L** (if present).
   * Under **OVRRightHandSynthetic/OVRRightHandVisual**, delete **OpenXRRightHand** and **OculusHand\_R** (if present).
5. **Add the custom hand prefabs**

   * Drag **OpenXRCustomHandPrefab\_L** under **OVRLeftHandSynthetic/OVRLeftHandVisual**.
   * Drag **OpenXRCustomHandPrefab\_R** under **OVRRightHandSynthetic/OVRRightHandVisual**.
6. **Adjust Hand Visual settings**

   * Select **OVRLeftHandVisual** → in **Hand Visual**, enable **Update Root Pose** and **Update Root Scale**.
   * For **Open XR Skinned Mesh Renderer**, assign the **LeftHand** (the child with **Skinned Mesh Renderer**) under **OpenXRCustomHandPrefab\_L/OpenXRLeftHand**.
   * Select **OpenXRCustomHandPrefab\_L** → in **OVR Custom Skeleton**, enable **Update Root Pose** and **Update Root Scale**.
   * Repeat for **OVRRightHandVisual** and **OpenXRCustomHandPrefab\_R** (assign **RightHand** with **Skinned Mesh Renderer**; enable **Update Root Pose** and **Update Root Scale**).
7. **Link DataModifier components**

   * In **OVRLeftHandSynthetic**, find **DataModifier | Modify Data From Source** and assign **OVRHands** (child of **OVRInteractionComprehensive**).
   * A small UI appears. Use the guide below to set values for **Left** and **Right**.
   * Repeat the same for **OVRRightHandSynthetic**.

   <div align="center">
     <img src="Data/Images/ovrhands.png" alt="OVRHands" width="200" />
   </div>

### Steps — Full Body (Movement SDK)

1. **Enable body tracking** on the rig

   * Select **OVRCameraRig** → **Inspector** → **OVRManager**.
   * Under **Tracking**, set **Tracking Origin Type = Floor Level**.
   * Under **Quest Features**, set **Body Tracking Support = Required**.
   * Under **Movement Tracking**, set **Body Tracking Joint Set = Full Body**.
2. **Retarget Y Bot**

   * In **Project**, right‑click the **Y Bot** model asset.
   * Choose **Movement SDK → Body Tracking → Open Retargeting Configuration Editor**.
   * Click through the **Next** steps → **Validate and save config** → **Done**.
   * Right‑click **Y Bot** again → **Movement SDK → Body Tracking → Add Character Retargeter**.
   * Again, click through **Next** → **Validate and save config** → **Done**.

### Verify — Meta SDK

1. Turn on the **Meta Simulator** toggle → click **Play**.
2. Full body: **Inputs → Movement Tracking Controls → Play random movement**.
3. Hands: use **WASD** or click for **pinch**.
4. Compare with the **Meta SDK** reference clips above.

---

## Troubleshooting (Quick Fixes)

* **I don’t see the Simulator panel.** Stop Play mode. Toggle the **Meta Simulator** off and on, then press **Play** again.
* **Hands don’t move.** Recheck that **OVRHands** is assigned to both **DataModifier** components (Meta task) or that **Two Hands Retargeter** input is the **TrackingSystem** (UnifiedXRMotion task).
* **Y Bot doesn’t move.** Confirm **Body Tracking** is enabled (Meta task) or that **Meta Body Tracking** feeds into **Meta Body Retargeter** with **Motion Avatar = Y Bot** (UnifiedXRMotion task).
* **Pink/missing models.** Ensure the **OpenXRCustomHandPrefab\_L/R** and **Y Bot** objects are present in the **Hierarchy** and their materials are assigned in the **Inspector**.

---

## After You Finish

1. Compare your scene behavior to the reference clips.
2. If your result matches, tell the administrator you are done.
3. Complete the questionnaires: **NASA‑TLX** and **SUS**.
4. Submit the survey here: **[Study Survey Link](https://docs.google.com/forms/d/e/1FAIpQLSeOqFWpfYdhKt8Hf3-RucCyR9Qm7beGRRzhLZ2IWdvW0Bi3Mw/viewform?usp=header&utm_source=chatgpt.com)**.

> Focus on **accuracy** and **task completion**, not speed.

---

## Version & Date

* **Unity**: 6000.0.33f1
* **Meta XR Simulator**: 77.0.0
* **Movement SDK**: 78.0.0 (Meta condition)
* Manual last updated: **September 3, 2025**
