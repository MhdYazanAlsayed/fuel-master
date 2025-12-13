import React, { useCallback, useMemo, useEffect, useRef } from "react";
import ReactFlow, {
  Node,
  Edge,
  Controls,
  Background,
  MiniMap,
  useNodesState,
  useEdgesState,
  ConnectionMode,
  Panel,
  ReactFlowInstance,
} from "reactflow";
import "reactflow/dist/style.css";

// Custom Employee Node Component
const EmployeeNode = ({ data }: { data: any }) => {
  const getRoleColor = (role: string) => {
    if (role === "CEO" || role === "CTO") return "#1e40af";
    if (role === "Director") return "#2563eb";
    if (role === "Manager" || role.includes("Manager")) return "#10b981";
    return "#8b5cf6";
  };

  const roleColor = getRoleColor(data.role);

  return (
    <div
      className="employee-node"
      style={{
        background: "white",
        border: `2px solid ${roleColor}`,
        borderRadius: "12px",
        padding: "1rem",
        minWidth: "200px",
        boxShadow: "0 4px 12px rgba(0, 0, 0, 0.1)",
        transition: "all 0.2s",
      }}
    >
      <div
        style={{
          display: "flex",
          alignItems: "center",
          gap: "0.75rem",
          marginBottom: "0.5rem",
        }}
      >
        <img
          src={data.avatar}
          alt={data.name}
          style={{
            width: "48px",
            height: "48px",
            borderRadius: "50%",
            border: `2px solid ${roleColor}`,
            objectFit: "cover",
          }}
        />
        <div style={{ flex: 1 }}>
          <div
            style={{
              fontWeight: 600,
              fontSize: "0.95rem",
              color: "#1e293b",
              marginBottom: "0.25rem",
            }}
          >
            {data.name}
          </div>
          <div
            style={{
              fontSize: "0.85rem",
              color: roleColor,
              fontWeight: 600,
            }}
          >
            {data.role}
          </div>
        </div>
      </div>
      {data.station && (
        <div
          style={{
            fontSize: "0.75rem",
            color: "#64748b",
            marginTop: "0.5rem",
            paddingTop: "0.5rem",
            borderTop: "1px solid #e2e8f0",
          }}
        >
          📍 {data.station}
        </div>
      )}
    </div>
  );
};

const nodeTypes = {
  employee: EmployeeNode,
};

export function HierarchyChart() {
  // Define the hierarchy structure
  const initialNodes: Node[] = useMemo(
    () => [
      // Executive Level (Top) - CEO and CTO
      {
        id: "ceo",
        type: "employee",
        position: { x: 300, y: 0 },
        data: {
          name: "Chief Executive Officer",
          role: "CEO",
          avatar:
            "https://ui-avatars.com/api/?name=CEO&background=1e40af&color=fff",
        },
      },
      {
        id: "cto",
        type: "employee",
        position: { x: 700, y: 0 },
        data: {
          name: "Chief Technology Officer",
          role: "CTO",
          avatar:
            "https://ui-avatars.com/api/?name=CTO&background=1e40af&color=fff",
        },
      },
      // Directors Level
      {
        id: "ops-director",
        type: "employee",
        position: { x: 0, y: 200 },
        data: {
          name: "Operations Director",
          role: "Director",
          avatar:
            "https://ui-avatars.com/api/?name=Operations+Director&background=2563eb&color=fff",
        },
      },
      {
        id: "hr-director",
        type: "employee",
        position: { x: 300, y: 200 },
        data: {
          name: "HR Director",
          role: "Director",
          avatar:
            "https://ui-avatars.com/api/?name=HR+Director&background=2563eb&color=fff",
        },
      },
      {
        id: "finance-director",
        type: "employee",
        position: { x: 600, y: 200 },
        data: {
          name: "Finance Director",
          role: "Director",
          avatar:
            "https://ui-avatars.com/api/?name=Finance+Director&background=2563eb&color=fff",
        },
      },
      {
        id: "tech-director",
        type: "employee",
        position: { x: 900, y: 200 },
        data: {
          name: "Technology Director",
          role: "Director",
          avatar:
            "https://ui-avatars.com/api/?name=Technology+Director&background=2563eb&color=fff",
        },
      },
      // Managers Level
      {
        id: "manager-1",
        type: "employee",
        position: { x: -150, y: 400 },
        data: {
          name: "John Smith",
          role: "Station Manager",
          station: "Main Street Station",
          avatar:
            "https://ui-avatars.com/api/?name=John+Smith&background=10b981&color=fff",
        },
      },
      {
        id: "manager-2",
        type: "employee",
        position: { x: 150, y: 400 },
        data: {
          name: "Sarah Johnson",
          role: "Station Manager",
          station: "Downtown Station",
          avatar:
            "https://ui-avatars.com/api/?name=Sarah+Johnson&background=10b981&color=fff",
        },
      },
      {
        id: "manager-3",
        type: "employee",
        position: { x: 450, y: 400 },
        data: {
          name: "Emily Brown",
          role: "HR Manager",
          avatar:
            "https://ui-avatars.com/api/?name=Emily+Brown&background=10b981&color=fff",
        },
      },
      {
        id: "manager-4",
        type: "employee",
        position: { x: 750, y: 400 },
        data: {
          name: "Finance Manager",
          role: "Finance Manager",
          avatar:
            "https://ui-avatars.com/api/?name=Finance+Manager&background=10b981&color=fff",
        },
      },
      {
        id: "manager-5",
        type: "employee",
        position: { x: 1050, y: 400 },
        data: {
          name: "IT Manager",
          role: "IT Manager",
          avatar:
            "https://ui-avatars.com/api/?name=IT+Manager&background=10b981&color=fff",
        },
      },
      // Staff Level
      {
        id: "staff-1",
        type: "employee",
        position: { x: -300, y: 600 },
        data: {
          name: "Mike Davis",
          role: "Supervisor",
          avatar:
            "https://ui-avatars.com/api/?name=Mike+Davis&background=8b5cf6&color=fff",
        },
      },
      {
        id: "staff-2",
        type: "employee",
        position: { x: -150, y: 600 },
        data: {
          name: "David Wilson",
          role: "Cashier",
          avatar:
            "https://ui-avatars.com/api/?name=David+Wilson&background=8b5cf6&color=fff",
        },
      },
      {
        id: "staff-3",
        type: "employee",
        position: { x: 0, y: 600 },
        data: {
          name: "Lisa Anderson",
          role: "Attendant",
          avatar:
            "https://ui-avatars.com/api/?name=Lisa+Anderson&background=8b5cf6&color=fff",
        },
      },
      {
        id: "staff-4",
        type: "employee",
        position: { x: 150, y: 600 },
        data: {
          name: "Tom Harris",
          role: "Cashier",
          avatar:
            "https://ui-avatars.com/api/?name=Tom+Harris&background=8b5cf6&color=fff",
        },
      },
      {
        id: "staff-5",
        type: "employee",
        position: { x: 450, y: 600 },
        data: {
          name: "Anna Martinez",
          role: "Recruiter",
          avatar:
            "https://ui-avatars.com/api/?name=Anna+Martinez&background=8b5cf6&color=fff",
        },
      },
      {
        id: "staff-6",
        type: "employee",
        position: { x: 750, y: 600 },
        data: {
          name: "Robert Taylor",
          role: "Accountant",
          avatar:
            "https://ui-avatars.com/api/?name=Robert+Taylor&background=8b5cf6&color=fff",
        },
      },
      {
        id: "staff-7",
        type: "employee",
        position: { x: 1050, y: 600 },
        data: {
          name: "Jennifer Lee",
          role: "Developer",
          avatar:
            "https://ui-avatars.com/api/?name=Jennifer+Lee&background=8b5cf6&color=fff",
        },
      },
    ],
    []
  );

  const initialEdges: Edge[] = useMemo(
    () => [
      // Executive Level: CEO and CTO connection (if needed, or they can both report to board)
      // CEO to Directors
      {
        id: "ceo-ops",
        source: "ceo",
        target: "ops-director",
        type: "smoothstep",
        animated: true,
        style: { stroke: "#2563eb", strokeWidth: 2 },
      },
      {
        id: "ceo-hr",
        source: "ceo",
        target: "hr-director",
        type: "smoothstep",
        animated: true,
        style: { stroke: "#2563eb", strokeWidth: 2 },
      },
      {
        id: "ceo-finance",
        source: "ceo",
        target: "finance-director",
        type: "smoothstep",
        animated: true,
        style: { stroke: "#2563eb", strokeWidth: 2 },
      },
      // CTO to Technology Director
      {
        id: "cto-tech",
        source: "cto",
        target: "tech-director",
        type: "smoothstep",
        animated: true,
        style: { stroke: "#2563eb", strokeWidth: 2 },
      },
      // Directors to Managers
      {
        id: "ops-m1",
        source: "ops-director",
        target: "manager-1",
        type: "smoothstep",
        animated: true,
        style: { stroke: "#10b981", strokeWidth: 2 },
      },
      {
        id: "ops-m2",
        source: "ops-director",
        target: "manager-2",
        type: "smoothstep",
        animated: true,
        style: { stroke: "#10b981", strokeWidth: 2 },
      },
      {
        id: "hr-m3",
        source: "hr-director",
        target: "manager-3",
        type: "smoothstep",
        animated: true,
        style: { stroke: "#10b981", strokeWidth: 2 },
      },
      {
        id: "finance-m4",
        source: "finance-director",
        target: "manager-4",
        type: "smoothstep",
        animated: true,
        style: { stroke: "#10b981", strokeWidth: 2 },
      },
      {
        id: "tech-m5",
        source: "tech-director",
        target: "manager-5",
        type: "smoothstep",
        animated: true,
        style: { stroke: "#10b981", strokeWidth: 2 },
      },
      // Managers to Staff
      {
        id: "m1-s1",
        source: "manager-1",
        target: "staff-1",
        type: "smoothstep",
        animated: true,
        style: { stroke: "#8b5cf6", strokeWidth: 2 },
      },
      {
        id: "m1-s2",
        source: "manager-1",
        target: "staff-2",
        type: "smoothstep",
        animated: true,
        style: { stroke: "#8b5cf6", strokeWidth: 2 },
      },
      {
        id: "m2-s3",
        source: "manager-2",
        target: "staff-3",
        type: "smoothstep",
        animated: true,
        style: { stroke: "#8b5cf6", strokeWidth: 2 },
      },
      {
        id: "m2-s4",
        source: "manager-2",
        target: "staff-4",
        type: "smoothstep",
        animated: true,
        style: { stroke: "#8b5cf6", strokeWidth: 2 },
      },
      {
        id: "m3-s5",
        source: "manager-3",
        target: "staff-5",
        type: "smoothstep",
        animated: true,
        style: { stroke: "#8b5cf6", strokeWidth: 2 },
      },
      {
        id: "m4-s6",
        source: "manager-4",
        target: "staff-6",
        type: "smoothstep",
        animated: true,
        style: { stroke: "#8b5cf6", strokeWidth: 2 },
      },
      {
        id: "m5-s7",
        source: "manager-5",
        target: "staff-7",
        type: "smoothstep",
        animated: true,
        style: { stroke: "#8b5cf6", strokeWidth: 2 },
      },
    ],
    []
  );

  const [nodes, setNodes, onNodesChange] = useNodesState(initialNodes);
  const [edges, setEdges, onEdgesChange] = useEdgesState(initialEdges);
  const reactFlowInstance = useRef<ReactFlowInstance | null>(null);

  const onConnect = useCallback(
    (params: any) => setEdges((eds) => [...eds, params]),
    [setEdges]
  );

  const onInit = useCallback((instance: ReactFlowInstance) => {
    reactFlowInstance.current = instance;
    // Fit view after a short delay to ensure nodes are rendered
    setTimeout(() => {
      instance.fitView({ padding: 0.2, duration: 400 });
    }, 100);
  }, []);

  return (
    <div className="admin-content">
      <div className="page-header">
        <div>
          <h1 className="page-title">Organization Hierarchy</h1>
          <p className="page-description">
            Interactive organizational structure - Zoom, pan, and explore your
            team
          </p>
        </div>
        <div className="page-actions">
          <button className="btn btn-outline-primary">
            <span className="btn-icon">📥</span>
            Export Chart
          </button>
          <button className="btn btn-primary">
            <span className="btn-icon">✏️</span>
            Edit Structure
          </button>
        </div>
      </div>

      <div
        className="content-card"
        style={{
          padding: 0,
          height: "calc(100vh - 300px)",
          minHeight: "600px",
        }}
      >
        <div style={{ width: "100%", height: "100%", position: "relative" }}>
          <ReactFlow
            nodes={nodes}
            edges={edges}
            onNodesChange={onNodesChange}
            onEdgesChange={onEdgesChange}
            onConnect={onConnect}
            onInit={onInit}
            nodeTypes={nodeTypes}
            connectionMode={ConnectionMode.Loose}
            attributionPosition="bottom-left"
            style={{ background: "#fafbfc" }}
            defaultViewport={{ x: 0, y: 0, zoom: 0.8 }}
            minZoom={0.1}
            maxZoom={2}
          >
            <Controls
              showInteractive={false}
              style={{
                background: "white",
                border: "1px solid #e2e8f0",
                borderRadius: "8px",
                boxShadow: "0 2px 8px rgba(0, 0, 0, 0.1)",
              }}
            />
            <MiniMap
              nodeColor={(node) => {
                if (node.data?.role === "CEO" || node.data?.role === "CTO")
                  return "#1e40af";
                if (node.data?.role === "Director") return "#2563eb";
                if (node.data?.role?.includes("Manager")) return "#10b981";
                return "#8b5cf6";
              }}
              maskColor="rgba(0, 0, 0, 0.1)"
              style={{
                background: "white",
                border: "1px solid #e2e8f0",
                borderRadius: "8px",
              }}
            />
            <Background color="#e2e8f0" gap={20} size={1} />
            <Panel
              position="top-left"
              style={{
                background: "white",
                padding: "0.75rem",
                borderRadius: "8px",
                boxShadow: "0 2px 8px rgba(0, 0, 0, 0.1)",
                border: "1px solid #e2e8f0",
              }}
            >
              <div
                style={{
                  fontSize: "0.85rem",
                  color: "#64748b",
                  marginBottom: "0.5rem",
                  fontWeight: 600,
                }}
              >
                Navigation Tips:
              </div>
              <div
                style={{
                  fontSize: "0.75rem",
                  color: "#64748b",
                  lineHeight: "1.6",
                }}
              >
                • Scroll to zoom in/out
                <br />
                • Click & drag to pan
                <br />• Use controls to reset view
              </div>
            </Panel>
          </ReactFlow>
        </div>
      </div>

      <div
        className="hierarchy-legend"
        style={{
          marginTop: "1.5rem",
          padding: "1rem",
          background: "white",
          borderRadius: "8px",
          border: "1px solid #e2e8f0",
        }}
      >
        <div style={{ display: "flex", gap: "2rem", flexWrap: "wrap" }}>
          <div
            className="legend-item"
            style={{ display: "flex", alignItems: "center", gap: "0.5rem" }}
          >
            <span
              className="legend-color ceo"
              style={{
                width: "16px",
                height: "16px",
                borderRadius: "4px",
                background: "#1e40af",
                display: "inline-block",
              }}
            ></span>
            <span style={{ fontSize: "0.9rem", color: "#475569" }}>
              Executive
            </span>
          </div>
          <div
            className="legend-item"
            style={{ display: "flex", alignItems: "center", gap: "0.5rem" }}
          >
            <span
              className="legend-color director"
              style={{
                width: "16px",
                height: "16px",
                borderRadius: "4px",
                background: "#2563eb",
                display: "inline-block",
              }}
            ></span>
            <span style={{ fontSize: "0.9rem", color: "#475569" }}>
              Directors
            </span>
          </div>
          <div
            className="legend-item"
            style={{ display: "flex", alignItems: "center", gap: "0.5rem" }}
          >
            <span
              className="legend-color manager"
              style={{
                width: "16px",
                height: "16px",
                borderRadius: "4px",
                background: "#10b981",
                display: "inline-block",
              }}
            ></span>
            <span style={{ fontSize: "0.9rem", color: "#475569" }}>
              Managers
            </span>
          </div>
          <div
            className="legend-item"
            style={{ display: "flex", alignItems: "center", gap: "0.5rem" }}
          >
            <span
              className="legend-color staff"
              style={{
                width: "16px",
                height: "16px",
                borderRadius: "4px",
                background: "#8b5cf6",
                display: "inline-block",
              }}
            ></span>
            <span style={{ fontSize: "0.9rem", color: "#475569" }}>Staff</span>
          </div>
        </div>
      </div>
    </div>
  );
}
