# Participant Manuals — Unity SDK Study (IEEE VR 2026)

The two manuals below describe identical tasks performed with two different SDKs. Instructions are written in plain language, with the same structure and level of detail to support fair comparison.

---

## Manual A — Our SDK (UnifiedXRMotion)

### Overview

You will prepare a Unity scene so an avatar follows simulated body and hand movement. You will complete **Full Body** and **Hands Only** tasks, then verify motion in the Meta Simulator.

### Before You Start

- **Unity version:** 6000.0.33f1
    
- **Provided & already installed:** UnifiedXRMotion, OpenXR (Desktop → Meta XR Feature Group enabled), Meta Core SDK, Meta XR Simulator (77.0.0).
    
- **Scene:** A scene containing the **Y Bot** model and a **Hands** object is open. If anything is missing or looks different, pause and ask the facilitator.
    

> Tip: If you make a mistake, press **Ctrl+Z** to undo.

---

### Part 1 — Full Body Tracking

**Goal:** Make **Y Bot** move with simulated full‑body motion.

**Steps**

1. **Attach the avatar controller**
    
    1. In the **Hierarchy**, select **Y Bot**.
        
    2. In the **Inspector**, click **Add Component** → add **Motion Avatar**.
        
    3. (No change needed for Body Type here.)
        
2. **Create the motion system objects**
    
    1. In the **Hierarchy**, right‑click → **Create Empty**. Name it **MotionSystem**.
        
    2. With **MotionSystem** selected, in **Inspector** click **Add Component** → add **Motion System**.
        
    3. Right‑click **MotionSystem** → **Create Empty**. Name it **TrackingSystem**.
        
    4. With **TrackingSystem** selected, add **Tracking Manager** and **Meta Body Tracking**.
        
    5. Right‑click **MotionSystem** → **Create Empty**. Name it **RetargetSystem**.
        
    6. With **RetargetSystem** selected, add **Retarget System** and **Meta Body Retargeter**.
        
3. **Connect the systems**
    
    1. Select **MotionSystem**. In the **Motion System** component, add **TrackingSystem** to the **Motion Triggers** list.
        
    2. Select **RetargetSystem**. In **Retarget System**, set **Motion Avatar** to **Y Bot** (drag from Hierarchy if helpful).
        
    3. In **Retarget System**, add **Meta Body Retargeter** to **Input Motions**.
        
    4. Select **Meta Body Retargeter** (on **RetargetSystem**). Set its **Input Motion** to **TrackingSystem**.
        

**Expected Outcome**

- When simulated movement plays, **Y Bot** should follow full‑body motion smoothly (head, torso, arms, and legs move).
    

---

### Part 2 — Hands‑Only Tracking

**Goal:** Make the **Hands** object move with simulated hand motion.

**Steps**

1. **Ensure the hands avatar is configured**
    
    1. In the **Hierarchy**, select **Hands**.
        
    2. If **Motion Avatar** is not present, click **Add Component** → add **Motion Avatar**.
        
    3. In **Motion Avatar**, set **Body Type** to **Two Hands**.
        
2. **Create the motion system objects** (separate from Part 1 for clarity)
    
    1. In the **Hierarchy**, create **MotionSystem** (Empty) and add **Motion System** to it.
        
    2. Create child **TrackingSystem** and add **Tracking Manager** and **Meta Hand Tracking**.
        
    3. Create child **RetargetSystem** and add **Retarget System** and **Two Hands Retargeter**.
        
3. **Connect the systems**
    
    1. On **MotionSystem**, add **TrackingSystem** to **Motion Triggers**.
        
    2. On **Retarget System** (under **RetargetSystem**), set **Motion Avatar** to **Hands**.
        
    3. In **Retarget System**, add **Two Hands Retargeter** to **Input Motions**.
        
    4. On **Two Hands Retargeter**, set its **Input Motion** to **TrackingSystem**.
        

**Expected Outcome**

- When simulated movement plays, the **Hands** object mirrors left/right hand poses and basic gestures.
    

---

### Verify With Meta Simulator (for both Parts)

**Steps**

1. In the Unity toolbar, click the **Meta Simulator** toggle (to the left of **Play**) to enable it.
    
2. Click **Play**.
    
3. In the Simulator controls, open **Inputs → Movement Tracking Controls → Play random movement**.
    

**Expected Outcome**

- The selected avatar (Full Body or Hands) should animate in place following the random motion.
    
- If there is no movement, stop Play, re‑check the connections above, and try again.
    

---

### End Note

Please focus on **completing** each part of the task, not on speed. After finishing, you will complete short surveys (SUS and NASA‑TLX) about your experience.

---

## Manual B — Meta SDK (Meta Core + Meta XR Interaction + Movement SDK)

### Overview

You will prepare a Unity scene so an avatar follows simulated body and hand movement using Meta’s SDKs. You will complete **Full Body** and **Hands Only** tasks, then verify motion in the Meta Simulator.

### Before You Start

- **Unity version:** 6000.0.33f1
    
- **Provided & already installed:** OpenXR (Desktop → Meta XR Feature Group enabled), Meta Core SDK, Meta XR Simulator (77.0.0), **Movement SDK (78.0.0)** (from `https://github.com/oculus-samples/Unity-Movement.git`), **Meta XR Interaction SDK**.
    
- **Scene:** A scene containing the **Y Bot** model is open.
    

> Tip: If you make a mistake, press **Ctrl+Z** to undo.

---

### Part 1 — Full Body Tracking (Movement SDK)

**Goal:** Make **Y Bot** move with simulated full‑body motion.

**Steps**

1. **Add the camera rig**
    
    1. In **Project** → **Packages/Meta XR Core SDK/Prefabs**, drag **OVRCameraRig** into the **Hierarchy**.
        
2. **Enable body tracking in OVRManager**
    
    1. Select **OVRCameraRig**. In **Inspector**, find **OVRManager**.
        
    2. Under **Quest Features**, set **Body Tracking Support** to **Required**.
        
    3. Under **Movement Tracking**, set **Body Tracking Joint Set** to **Full Body**.
        
3. **Retarget Y Bot to body tracking**
    
    1. In **Project**, right‑click the **Y Bot** model asset.
        
    2. Choose **Movement SDK → Body Tracking → Open Retargeting Configuration Editor**.
        
    3. If prompted to create a missing config JSON, choose **Create** and save it.
        
    4. Right‑click the **Y Bot** model asset again → **Movement SDK → Body Tracking → Add Character Retargeter**.
        

**Expected Outcome**

- When simulated movement plays, **Y Bot** should follow full‑body motion smoothly.
    

---

### Part 2 — Hands‑Only Tracking (Interaction SDK)

**Goal:** Make hands move with simulated hand motion.

**Steps**

1. **Add hand interaction prefabs**
    
    1. In **Project** → **Packages/Meta XR Interaction SDK/Runtime/Prefabs**, drag **OVRInteractionComprehensive** as a **child** of **OVRCameraRig**.
        
    2. In the Hierarchy under **OVRInteractionComprehensive**, remove any prefabs you don’t need so that **OVRHands** remains.
        
2. **Add synthetic hands for simulation**
    
    1. In **Project** → **Packages/Meta XR Interaction SDK Essentials/Runtime/Prefabs/Hands**, add **OVRLeftHandSynthetic** and **OVRRightHandSynthetic** as children under **OVRCameraRig** (or alongside the hands setup).
        
    2. Select **OVRLeftHandSynthetic**. In **Inspector**, set **Data Modifier** to **OVRHands (Left Hand)**.
        
    3. Select **OVRRightHandSynthetic**. Set **Data Modifier** to **OVRHands (Right Hand)**.
        

**Expected Outcome**

- When simulated movement plays, the synthetic left and right hands animate and pose changes are visible.
    

---

### Verify With Meta Simulator (for both Parts)

**Steps**

1. In the Unity toolbar, click the **Meta Simulator** toggle (to the left of **Play**) to enable it.
    
2. Click **Play**.
    
3. In the Simulator controls, open **Inputs → Movement Tracking Controls → Play random movement**.
    

**Expected Outcome**

- The selected avatar (Full Body or Hands) should animate in place following the random motion.
    
- If there is no movement, stop Play, re‑check the settings above, and try again.
    

---

### End Note

Please focus on **completing** each part of the task, not on speed. After finishing, you will complete short surveys (SUS and NASA‑TLX) about your experience.