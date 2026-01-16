# Immersive Cosmology Explorer (ICE)

**A Unity-Based VR and Desktop Visualization System for Scientific Dataset Exploration**

**Project Documentation**: [Full Design Document](https://docs.google.com/document/d/your-doc-link)  
**Demo Video**: [Video Demonstration](https://www.youtube.com/watch?v=hxwp6dEfDHk)  
**Build Download**: [Unity Build](https://drive.google.com/your-build-link)

---

## Overview

**Immersive Cosmology Explorer (ICE)** is an advanced Unity-based VR and desktop visualization system designed to enable scientists and researchers to explore, analyze, and collaborate on complex 3D cosmological datasets. The system bridges the gap between immersive virtual reality environments and traditional desktop analysis tools, providing real-time synchronization between both platforms for collaborative scientific discovery.

ICE enables researchers to filter, annotate, and analyze particle data from large-scale cosmological simulations, transforming billions of data points into an interactive, intuitive 3D exploration environment. The system supports multiple particle types (gas, stars, dark matter), dynamic filtering, perceptually uniform colormaps, vector field visualization, and session management—all within an immersive VR experience that can be simultaneously monitored and controlled from a desktop interface.

---

## Problem Statement

Scientists working with large-scale spatial datasets generated in cosmology face significant challenges in exploring, interpreting, and communicating their data. Traditional 2D desktop-based visualization tools struggle to represent the intricate spatial relationships and multi-dimensional structures present in cosmological simulations. 

**Key Challenges Addressed:**
- Difficulty in visualizing complex 3D relationships between particle types
- Limited ability to filter visual noise while maintaining spatial context
- Inefficient workflows requiring constant switching between analysis tools
- Lack of collaborative environments for simultaneous VR and desktop users
- Need for real-time manipulation of multi-billion particle datasets

ICE solves these problems by providing an immersive, user-centered visualization environment that supports responsive interaction, multi-device collaboration, and scientific-grade data analysis. 

---

## Key Features

### Immersive VR Visualization
- **Large-Scale Point Cloud Rendering**: Efficiently renders millions of particles from cosmological simulations
- **Multi-Particle Type Support**: Toggle between gas, stars, dark matter, and other particle types
- **Vector Field Overlay**: Visualize particle velocities and directional flows with dynamic arrow overlays
- **Spatial Navigation**: Natural VR controller-based movement, rotation, and scaling of datasets
- **Perceptually Uniform Colormaps**: Scientific-grade color schemes for accurate data interpretation

### Advanced Data Manipulation
- **Scalar Filtering**: Define numeric ranges to filter particles based on physical properties
- **Real-Time Interaction**: Immediate visual feedback for all manipulation operations
- **Point Size and Opacity Control**: Adjust visualization parameters for clarity and focus
- **Region of Interest Selection**: Isolate and analyze specific spatial regions
- **Time-Lapse Playback**: Automatically cycle through simulation time steps to observe evolution

### Desktop-VR Synchronization
- **Real-Time Mirroring**: Desktop window displays current VR visualization
- **Asymmetric Collaboration**: VR and desktop users work simultaneously on the same dataset
- **Cross-Platform Controls**: Actions performed in one environment reflect instantly in the other
- **Shared Annotations**: Collaborative note-taking and marking visible across devices

### Session Management
- **Save and Load States**: Preserve complete visualization configurations including filters, colormaps, and annotations
- **Session Replay**: Return to previous analysis states for comparison
- **Export Capabilities**: Save screenshots, annotations, and data subsets
- **Iterative Workflows**: Support long-term scientific analysis with persistent sessions

---

## Technical Implementation

### Architecture and Design Patterns

**Three-Layer Architecture**

1. **Data Management Component**
   - Import, storage, and processing of point cloud datasets
   - Spatial indexing with octree structures for efficient rendering
   - Level-of-Detail (LOD) system for performance optimization
   - Cross-device data streaming between desktop and VR

2. **Visualization and Interaction Layer**
   - VR controller-based manipulation (grab, scale, rotate)
   - Desktop interface with mouse/keyboard controls
   - Contextual radial menus for VR interactions
   - Dynamic visual mappings and parameter controls

3. **Collaboration and Multi-Device Layer**
   - Real-time state synchronization across platforms
   - Shared session management with presence indicators
   - Annotation system with cross-device visibility
   - Session history and version control

**Design Principles**
- **Learnability**: Simple, controller-based interactions for non-expert VR users
- **Efficiency**: Streamlined workflows minimizing task interruptions
- **Accessibility**: Compatible with affordable, widely available VR headsets
- **Collaboration**: Real-time synchronization between heterogeneous devices

### Core Systems

#### Data Processing and Rendering
```
Assets/Scripts/
├── DataProcessing/
│   ├── ParticleDataLoader. cs      # Import and parse simulation data
│   ├── TimeStepManager.cs         # Handle temporal dataset navigation
│   ├── SpatialIndexing.cs         # Octree-based spatial queries
│   └── LODController.cs           # Level-of-detail optimization
```

**Key Features:**
- High-performance import utility for large particle files
- Automatic organization of data by time step
- Real-time 3D rendering with GPU acceleration
- Frustum culling and occlusion optimization

#### VR Interaction System
```
Assets/Scripts/VR/
├── VRController.cs                # Controller input handling
├── NavigationController.cs        # Spatial movement and teleportation
├── ManipulationTools.cs          # Data interaction tools
└── RadialMenu.cs                  # Contextual VR menu system
```

**Interaction Features:**
- Joystick-based navigation and rotation
- Trigger-based selection and confirmation
- Controller ray casting for UI interaction
- Haptic feedback for action confirmation

#### Desktop Interface
```
Assets/Scripts/Desktop/
├── DesktopCameraController.cs    # Mouse/keyboard navigation
├── UIManager.cs                   # Panel and menu management
├── SyncManager.cs                 # VR-desktop synchronization
└── ExportManager.cs               # Screenshot and data export
```

**Desktop Features:**
- Mirror view of VR visualization
- Independent camera controls
- Parameter adjustment panels
- Real-time sync with VR environment

#### Visualization Pipeline
```
Assets/Shaders/
├── PointCloud.shader              # Custom point rendering
├── VectorField.shader             # Arrow overlay visualization
└── Colormap.shader                # Perceptually uniform color mapping
```

**Visual Systems:**
- Custom shader-based point cloud rendering
- GPU-accelerated particle visualization
- Dynamic colormap application
- Vector field overlay with configurable arrow size

---

## Technologies and Tools

### Unity Systems
- **Unity 2022.3 LTS**:  Long-term support game engine
- **Universal Render Pipeline (URP)**: Optimized rendering pipeline
- **XR Interaction Toolkit**: VR controller input and interaction
- **TextMeshPro**: High-quality UI text rendering
- **Shader Graph**:  Visual shader authoring for custom effects

### VR Hardware Support
- **Meta Quest 2/3**: Standalone and PC VR modes
- **HTC Vive**: PC VR with full tracking
- **Compatible with OpenXR**: Standard VR platform support

### Programming
- **C# (. NET Standard 2.1)**: Primary programming language (67.1%)
- **ShaderLab**: Unity shader definitions (24%)
- **HTML**:  Documentation and UI components (5. 2%)
- **HLSL**: High-Level Shader Language (3.7%)

### Data Processing
- **Point Cloud Libraries**: Custom parsers for cosmological simulation formats
- **Spatial Data Structures**: Octree and KD-tree implementations
- **Real-Time Streaming**:  Efficient data transfer between components

---

## Project Structure

```
Immersive_Cosmology_Explorer/
├── Assets/
│   ├── Scripts/
│   │   ├── DataProcessing/
│   │   │   ├── ParticleDataLoader.cs
│   │   │   ├── TimeStepManager.cs
│   │   │   ├── SpatialIndexing. cs
│   │   │   └── LODController.cs
│   │   ├── VR/
│   │   │   ├── VRController.cs
│   │   │   ├── NavigationController.cs
│   │   │   ├── ManipulationTools.cs
│   │   │   └── RadialMenu.cs
│   │   ├── Desktop/
│   │   │   ├── DesktopCameraController.cs
│   │   │   ├── UIManager. cs
│   │   │   ├── SyncManager.cs
│   │   │   └── ExportManager.cs
│   │   └── Collaboration/
│   │       ├── SessionManager.cs
│   │       ├── AnnotationSystem.cs
│   │       └── StateSync.cs
│   ├── Shaders/
│   │   ├── PointCloud.shader
│   │   ├── VectorField.shader
│   │   └── Colormap.shader
│   ├── Materials/
│   ├── Prefabs/
│   └── Scenes/
├── Documentation/
│   ├── UserGuide.md
│   ├── DeveloperDocs.md
│   └── DesignDocument.pdf
├── ProjectSettings/
└── README.md
```

---

## Getting Started

### Prerequisites
- Unity Hub (latest version)
- Unity Editor 2022.3 LTS or newer
- VR Headset (Meta Quest 2/3 or HTC Vive)
- Windows 10/11 or macOS (for desktop mode)
- Minimum 16GB RAM (32GB recommended for large datasets)

### Installation

**Clone the Repository**
```bash
git clone https://github.com/Superkart/Immersive_Cosmology_Explorer.git
cd Immersive_Cosmology_Explorer
```

**Open in Unity Hub**
1. Open Unity Hub
2. Click "Add" and navigate to the cloned repository folder
3. Select Unity version 2022.3 LTS
4. Open the project

**Configure VR Settings**
1. Navigate to Edit → Project Settings → XR Plugin Management
2. Enable OpenXR or Oculus XR Plugin (depending on headset)
3. Configure interaction profiles for your VR controllers

**Load Sample Dataset**
1. Place cosmological simulation data in `Assets/StreamingAssets/Data/`
2. Open the main scene:  `Assets/Scenes/MainVisualization.unity`
3. Press Play to enter VR mode

---

## How to Use

### Loading and Visualizing Datasets

**Desktop Mode:**
1. Launch the application
2. Click "Load Dataset" from the main menu
3. Browse to select particle data files
4. Choose visualization parameters (particle types, colormap)
5. Click "Visualize" to render the dataset

**VR Mode:**
1. Put on VR headset
2. Use left controller menu to access "Load Data"
3. Select dataset using controller ray pointer
4. Confirm selection with trigger button
5. Dataset loads and renders in 3D space

### Manipulating and Filtering Data

**Controller-Based Interaction (VR):**
- **Navigation**: Use left joystick to move through space
- **Rotation**: Use right joystick to rotate dataset
- **Scaling**:  Grip buttons + move controllers apart/together
- **Filtering**: Open radial menu → Select "Filters" → Adjust ranges

**Desktop Interface:**
- **Camera Control**: Mouse drag to orbit, scroll to zoom
- **Filters**: Use slider controls in right panel
- **Particle Types**: Toggle checkboxes for gas/stars/dark matter
- **Colormap**: Select from dropdown menu

### Time-Lapse Playback

1.  Ensure dataset contains multiple time steps
2. Open "Time Control" panel (desktop) or menu (VR)
3. Click "Play" to auto-advance through time steps
4. Current time-step index displays in UI
5. Use slider to jump to specific time steps

### Saving and Loading Sessions

**Save Session:**
1. Open "Session" menu
2. Click "Save Current State"
3. Enter session name
4. Confirm save (preserves filters, colormaps, annotations)

**Load Session:**
1. Open "Session" menu
2. Select from list of saved sessions
3. Click "Load"
4. View restores to exact previous configuration

---

## User Tasks and Workflows

### Task 1: Dataset Exploration
1. Load 3D cosmological simulation dataset
2. Toggle between particle types (gas, stars, dark matter)
3. Rotate and zoom to observe spatial patterns
4. Enable vector arrows to visualize particle velocities
5. Adjust visualization scale and opacity

### Task 2: Data Filtering and Analysis
1. Open filter controls
2. Define value ranges for particle properties (density, temperature, mass)
3. Apply filters to hide particles outside thresholds
4. Zoom into filtered regions to examine substructures
5. Toggle between filtered and full views for context

### Task 3: Collaborative Analysis
1. Start VR session
2. Desktop collaborator joins via mirrored window
3. VR user navigates and manipulates dataset
4. Desktop user adjusts filters and colormaps
5. Both users add annotations visible in both environments

### Task 4: Session Management
1. Perform analysis and apply multiple filters
2. Add annotations to mark interesting structures
3. Save complete session state
4. Load session later to continue analysis
5. Export screenshots and data subsets for publication

---

## Code Highlights

### Point Cloud Rendering System
```csharp
public class ParticleDataLoader : MonoBehaviour
{
    private List<ParticleData> particles;
    private ComputeBuffer particleBuffer;
    
    public void LoadDataset(string filePath)
    {
        particles = ParseDataFile(filePath);
        InitializeRenderBuffer();
        UpdateVisualization();
    }
    
    private void InitializeRenderBuffer()
    {
        particleBuffer = new ComputeBuffer(particles.Count, 
            ParticleData.Size);
        particleBuffer.SetData(particles. ToArray());
        material.SetBuffer("_Particles", particleBuffer);
    }
}
```

### VR-Desktop Synchronization
```csharp
public class SyncManager :  MonoBehaviour
{
    private NetworkVariable<FilterState> sharedFilters;
    
    public void UpdateFilter(string paramName, float value)
    {
        if (IsVRMode)
            UpdateFilterServerRpc(paramName, value);
        else
            ApplyFilterLocally(paramName, value);
    }
    
    [ServerRpc]
    private void UpdateFilterServerRpc(string param, float val)
    {
        sharedFilters.Value = new FilterState(param, val);
        SyncToAllClientsClientRpc(param, val);
    }
}
```

### Time-Lapse Controller
```csharp
public class TimeStepManager : MonoBehaviour
{
    private int currentTimeStep = 0;
    private List<string> timeStepFiles;
    
    public void PlayTimeLapse(float intervalSeconds)
    {
        StartCoroutine(TimeLapseCoroutine(intervalSeconds));
    }
    
    private IEnumerator TimeLapseCoroutine(float interval)
    {
        while (currentTimeStep < timeStepFiles. Count)
        {
            LoadTimeStep(currentTimeStep);
            UpdateUITimeStepClientRpc(currentTimeStep);
            yield return new WaitForSeconds(interval);
            currentTimeStep++;
        }
    }
}
```

---

## Development Highlights

### What Makes This Project Stand Out

**Scientific Visualization Expertise**
- Custom point cloud rendering pipeline for billions of particles
- Perceptually uniform colormaps following scientific visualization standards
- Real-time data filtering with spatial indexing optimization
- Support for multiple data types and scientific file formats

**Advanced VR Implementation**
- Intuitive controller-based interaction for complex 3D data
- Comfortable navigation system preventing VR motion sickness
- Contextual radial menus for efficient parameter access
- Haptic feedback integration for enhanced user experience

**Cross-Platform Architecture**
- Real-time synchronization between VR and desktop environments
- Asymmetric collaboration supporting different user roles
- Shared state management with conflict resolution
- Session persistence and version control

**Performance Optimization**
- GPU-accelerated particle rendering with custom shaders
- Level-of-detail system for large-scale datasets
- Frustum culling and occlusion optimization
- Efficient spatial data structures (octree, KD-tree)

**User-Centered Design**
- Extensive formative evaluation with cognitive walkthroughs
- Iterative design refinement based on user feedback
- Alignment with Nielsen's usability heuristics
- Accessibility considerations for diverse user groups

---

## Evaluation and Testing

### Formative Evaluation Results

**Cognitive Walkthrough Findings:**
- Controller-based interaction emerged as most reliable method
- Need for stronger visual signifiers and feedback cues
- Improved terminology alignment with user mental models
- Enhanced confirmation messages for critical actions

**Key Usability Improvements:**
- Hover and selection feedback with color transitions (yellow → blue → green)
- Removal of ambiguous background colors behind text
- Shortened and simplified text labels
- Real-time value updates for sliders and parameter controls
- Directional arrows and on-screen rotation indicators

### Usability Heuristics Evaluation

**Nielsen's Heuristics Compliance:**
- **Visibility of System Status**: Color-coded mode indicators (yellow/blue/green)
- **Match Between System and Real World**: Familiar metaphors and recognizable symbols
- **User Control and Freedom**: Undo functionality and confirmation dialogs
- **Consistency and Standards**: Unified iconography and interaction patterns
- **Error Prevention**: Gradual visual feedback and highlighted affordances
- **Recognition Rather Than Recall**: Persistent status indicators
- **Flexibility and Efficiency**:  Multiple input modalities (keyboard, mouse, controllers)

---

## Performance Considerations

**Optimization Techniques:**
- GPU instancing for particle rendering
- Spatial partitioning with octree structures
- Frustum culling and occlusion queries
- Level-of-detail switching based on camera distance
- Asynchronous data loading and streaming

**System Requirements:**

| Component | Minimum | Recommended |
|-----------|---------|-------------|
| **CPU** | Intel i5-9400 | Intel i7-10700K |
| **GPU** | NVIDIA GTX 1060 | NVIDIA RTX 3070 |
| **RAM** | 16GB | 32GB |
| **Storage** | 500GB SSD | 1TB NVMe SSD |
| **VR Headset** | Meta Quest 2 | Meta Quest 3 / HTC Vive Pro |

---

## Future Enhancements

### Planned Features
- **AI-Assisted Analysis**:  Automatic structure detection and labeling
- **Multi-User Collaboration**: Support for more than two simultaneous users
- **Advanced Analytics**: Statistical computations and data export tools
- **Mobile VR Support**: Standalone Quest optimization
- **Cloud Integration**: Remote dataset streaming and collaboration
- **Machine Learning Integration**: Pattern recognition in cosmological structures
- **Voice Commands**:  Hands-free interaction for parameter adjustment
- **Gesture Recognition**: Natural hand tracking for manipulation

### Research Extensions
- Integration with other scientific visualization tools
- Support for additional dataset types (microscopy, fluid dynamics)
- Advanced rendering techniques (volume rendering, isosurfaces)
- Publication-ready visualization export
- AR mode for mixed-reality collaboration

---

## Known Limitations

**Current Constraints:**
- Maximum recommended dataset size: 10 million particles
- Desktop-VR sync has ~100ms latency under optimal network conditions
- Limited to two simultaneous users (VR + Desktop)
- Requires moderate VR experience for efficient navigation
- Some motion sensitivity for extended VR sessions

**Workarounds:**
- Use LOD system for datasets exceeding 10M particles
- Pre-filter data before loading for better performance
- Take breaks during long VR sessions to prevent fatigue

---

## Academic Context

**Course**:  CS522 Human-Computer Interaction  
**Institution**: University of Illinois Chicago  
**Semester**: Fall 2025  
**Instructor**: Prof. Debaleena Chattopadhyay

---

## Development Team

### Karthik Ragi - Lead Developer
**Primary Contributions:**
- Complete VR implementation and controller interaction systems
- Backend architecture and data processing pipeline
- System function implementation across all three architectural layers
- Performance optimization and custom shader development
- VR-desktop synchronization implementation
- Spatial indexing and LOD systems
- Technical problem-solving and debugging
- Integration testing and deployment

**Technical Responsibilities:**
- Designed and implemented VR interfaces including layout and spatial UI elements
- Built and tested major VR functionalities with iterative improvement
- Developed backend modules for data processing, storage, and cross-platform communication
- Optimized backend performance for VR rendering and real-time interaction
- Created custom point cloud rendering pipeline with GPU acceleration
- Implemented time-lapse playback and session management systems

### Hossein Fathollahian - Research & Evaluation Lead
**Primary Contributions:**
- Extensive data analysis and structure identification for system design
- User goal definition and task breakdown
- Literature review of three state-of-the-art research publications
- Created 30+ design sketches across multiple iteration stages
- Formative evaluation design and execution
- Cognitive walkthrough scenario and task development
- Project documentation and methodology development
- Weekly meeting management and team coordination

**Key Responsibilities:**
- Conducted data analysis to identify manipulation, filtering, and storage opportunities
- Defined user goals and detailed task sequences
- Completed comprehensive formative evaluation documentation
- Built initial wireframes for evaluation sessions
- Provided continuous development feedback and VR testing support

### Ahmad Albawaneh - Desktop UI & Requirements Specialist
**Primary Contributions:**
- Semi-structured user interviews for requirements gathering
- Desktop interface requirements definition and alignment
- User scenario development emphasizing desktop workflows
- Concept relationship diagrams with UI and information flow focus
- Desktop-specific design challenge identification
- Multiple desktop UI screen designs including menus, panels, and task flows
- Cognitive walkthrough preparation and execution

**Key Responsibilities:**
- Translated user feedback into actionable desktop interface requirements
- Designed desktop UI with visual hierarchy and accessibility principles
- Ensured cross-platform design coherence between desktop and VR
- Collaborated on backend compatibility for designed UI structures
- Participated in design discussions and integration planning

### Idunnuoluwa Adeniji - Conceptualization & Documentation Lead
**Primary Contributions:**
- System conceptualization and initial vision development
- User goals and requirements definition
- Design challenge identification and documentation
- Meeting coordination and scheduling
- Data provision and dataset organization
- Paper writing and formatting
- Critical analysis of controller mapping approaches
- Literature review of data visualization gaps
- Interaction and manipulation tool implementation guidance

**Key Responsibilities:**
- Provided scripts and shaders for data rendering support
- Conducted critical analysis of interaction design approaches
- Contributed to slide preparation and presentation materials
- Offered input on UI tool implementation decisions
- Provided feedback throughout all development phases

---

## Documentation and Resources

**Full Documentation**:  [Project Workbook (Google Docs)](https://docs.google.com/document/d/your-full-doc-link)

**Additional Resources:**
- [Sketches and Design Iterations](https://drive.google.com/drive/folders/1mNsg09_rCxjXKVI7U4Y3aeCRFkJtiGkV)
- [Cognitive Walkthrough Materials](https://drive.google.com/drive/folders/1DFWikIG7-66-hTG30DU4r6r-N7izYH8-)
- User Guide and Developer Documentation (included in repository)

---

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## Contact

**Project Repository**: [Immersive_Cosmology_Explorer](https://github.com/Superkart/Immersive_Cosmology_Explorer)

**Team Members:**
- Karthik Ragi - [@Superkart](https://github.com/Superkart)
- Hossein Fathollahian
- Ahmad Albawaneh
- Idunnuoluwa Adeniji

---

## Acknowledgments

- **Prof. Debaleena Chattopadhyay** for HCI guidance and feedback throughout the project
- **Dr. David Joiner** (Research Professor, Astrophysicist) for domain expertise, user testing, and scientific validation
- **Unity Technologies** for XR Interaction Toolkit and Universal Render Pipeline
- **Scientific visualization community** for colormap standards and best practices
- **Graduate research assistants** for participating in formative evaluation sessions

---

**Immersive Scientific Visualization | VR Data Exploration | Collaborative Research Tools**
