import { useState } from "react";
import OfficerList from "./pages/OfficerList";
import OfficerDetail from "./pages/OfficerDetail";
import CitizenView from "./pages/CitizenView";

const TABS = [
  { id: "list",    label: "פקיד · בקשות" },
  { id: "citizen", label: "מסך אזרח"      },
];

export default function App() {
  const [view, setView]              = useState("list");
  const [selectedAppId, setSelected] = useState(null);

  function openDetail(id) {
    setSelected(id);
    setView("detail");
  }

  return (
    <div className="bg-bg min-h-screen" dir="rtl">

      {/* Topbar */}
      <nav className="bg-surf border-b border-border shadow-sm px-7 flex items-center gap-1 sticky top-0 z-50">
        <div className="text-text font-black text-sm py-4 ml-5 shrink-0">
          ⚖️ מערכת החזרים
        </div>

        {TABS.map(tab => {
          const active = view === tab.id || (view === "detail" && tab.id === "list");
          return (
            <button
              key={tab.id}
              onClick={() => setView(tab.id)}
              className={`px-3.5 py-[18px] text-sm font-semibold border-b-2 transition-colors shrink-0
                ${active
                  ? "text-accent border-accent"
                  : "text-sub border-transparent hover:text-text"
                }`}
            >
              {tab.label}
            </button>
          );
        })}

      </nav>

      {/* Pages */}
      {view === "list"    && <OfficerList   onOpen={openDetail} />}
      {view === "detail"  && <OfficerDetail applicationId={selectedAppId} onBack={() => setView("list")}/>}
      {view === "citizen" && <CitizenView />}
    </div>
  );
}