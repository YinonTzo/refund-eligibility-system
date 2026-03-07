import { useState, useEffect } from "react";
import { getPendingApplications } from "../api";
import Avatar from "../components/Avatar";
import Badge from "../components/Badge";
import Spinner from "../components/Spinner";
import ErrorMsg from "../components/ErrorMsg";

export default function OfficerList({ onOpen }) {
  const [hov, setHov]         = useState(null);
  const [apps, setApps]       = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError]     = useState(null);

  async function load() {
    setLoading(true); setError(null);
    try { setApps(await getPendingApplications()); }
    catch (e) { setError(e.message); }
    finally { setLoading(false); }
  }

  useEffect(() => { load(); }, []);

  return (
    <div className="p-9 dir-rtl" dir="rtl">
      <div className="max-w-4xl mx-auto">

        <div className="mb-7">
          <h1 className="text-text text-xl font-extrabold mb-1">בקשות ממתינות</h1>
          <p className="text-sub text-sm">
            {loading ? "טוען..." : `${apps.length} בקשות מחכות לטיפולך`}
          </p>
        </div>

        {loading && <Spinner />}
        {error && <ErrorMsg message={error} onRetry={load} />}

        {!loading && !error && (
          <div className="flex flex-col gap-2.5">
            {apps.map(app => (
              <div
                key={app.applicationId}
                onClick={() => onOpen(app.applicationId)}
                onMouseEnter={() => setHov(app.applicationId)}
                onMouseLeave={() => setHov(null)}
                className={`bg-surf rounded-xl px-5 py-3.5 cursor-pointer transition-all duration-200 shadow-sm
                  ${hov === app.applicationId
                    ? "border border-accent shadow-lg -translate-y-0.5"
                    : "border border-border"
                  }`}
              >
                <div className="flex items-center gap-4">
                  <Avatar name={app.citizenFullName} size={38} />

                  <div className="w-44 shrink-0">
                    <div className="text-text font-bold text-sm">{app.citizenFullName}</div>
                    <div className="text-sub text-xs font-mono tracking-wide mt-0.5">{app.citizenIdentityNumber}</div>
                  </div>

                  <div className="w-px h-8 bg-border shrink-0" />

                  <div className="w-24 shrink-0">
                    <div className="text-sub text-[9px] font-bold tracking-widest uppercase mb-1">שנת מס</div>
                    <div className="bg-accent-l text-accent font-black text-sm px-2 py-0.5 rounded-md inline-block">
                      {app.taxYear}
                    </div>
                  </div>

                  <div className="w-px h-8 bg-border shrink-0" />

                  <div className="w-24 shrink-0">
                    <div className="text-sub text-[9px] font-bold tracking-widest uppercase mb-1">הוגש</div>
                    <div className="text-sub text-xs">{new Date(app.createdAt).toLocaleDateString("he-IL")}</div>
                    <div className="text-dim text-[10px]">{new Date(app.createdAt).toLocaleTimeString("he-IL", { hour: "2-digit", minute: "2-digit" })}</div>
                  </div>

                  <div className="flex-1" />

                  <Badge status="Pending" />

                  <div className={`text-xs font-semibold flex items-center gap-1 shrink-0 transition-colors ${hov === app.applicationId ? "text-accent" : "text-sub"}`}>
                    פתח לטיפול
                    <span className="inline-block scale-x-[-1]">→</span>
                  </div>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}