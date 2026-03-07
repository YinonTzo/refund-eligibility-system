import { useState } from "react";
import { getCitizenHistory } from "../api";
import Avatar from "../components/Avatar";
import Badge from "../components/Badge";
import Card from "../components/Card";
import SectionHead from "../components/SectionHead";

export default function CitizenView() {
  const [id, setId]      = useState("");
  const [data, setData]  = useState(null);
  const [err, setErr]    = useState(null);
  const [loading, setLd] = useState(false);

  async function search() {
    setErr(null); setData(null);
    if (id.length !== 9) { setErr("מספר זהות חייב להכיל 9 ספרות"); return; }
    setLd(true);
    try { setData(await getCitizenHistory(id)); }
    catch (e) { setErr(e.message); }
    finally { setLd(false); }
  }

  return (
    <div className="px-7 pt-14 pb-10" dir="rtl">
      <div className="max-w-lg mx-auto">

        <div className="text-center mb-9">
          <div className="text-5xl mb-3">🏛️</div>
          <h1 className="text-text text-2xl font-black mb-2 tracking-tight">בדיקת זכאות להחזר</h1>
          <p className="text-sub text-sm">הכנס מספר זהות לצפייה בסטטוס הבקשה שלך</p>
        </div>

        <Card className="p-4 mb-5">
          <div className="flex gap-2.5">
            <input
              value={id}
              onChange={e => setId(e.target.value.replace(/\D/g, "").slice(0, 9))}
              onKeyDown={e => e.key === "Enter" && search()}
              placeholder="000000000"
              className="flex-1 bg-bg border border-border rounded-xl px-4 py-3 text-text text-lg font-mono tracking-widest outline-none focus:border-accent transition-colors"
            />
            <button
              onClick={search}
              className="bg-accent text-white border-none rounded-xl px-5 py-3 font-bold text-sm hover:opacity-90 transition-opacity shadow-md"
            >
              {loading ? "⏳" : "חיפוש"}
            </button>
          </div>
          {err && <div className="text-red text-sm mt-2">{err}</div>}
        </Card>

        {data && (() => {
          const app = data.latestApplication;
          const isApproved = app?.status === "Approved";
          const isRejected = app?.status === "Rejected";
          return (
            <>
              <Card className="p-4 mb-3.5 flex items-center gap-3">
                <Avatar name={data.citizen.fullName} size={42} />
                <div>
                  <div className="text-text font-extrabold text-base">{data.citizen.fullName}</div>
                  <div className="text-sub text-xs font-mono mt-0.5">ת.ז. {data.citizen.identityNumber}</div>
                </div>
              </Card>

              {app && (
                <Card className={`p-5 mb-3.5 ${isApproved ? "border-green-b" : isRejected ? "border-red-b" : "border-border"}`}>
                  <div className="flex justify-between items-start mb-3">
                    <div>
                      <div className="text-sub text-[10px] font-bold tracking-widest uppercase mb-1">בקשה אחרונה</div>
                      <div className="text-text font-extrabold text-lg">שנת מס {app.taxYear}</div>
                    </div>
                    <Badge status={app.status} forCitizen />
                  </div>

                  {isApproved && (
                    <div className="bg-green-l border border-green-b rounded-xl px-4 py-3.5 flex justify-between items-center">
                      <div>
                        <div className="text-sub text-[10px] font-bold tracking-widest uppercase mb-1">סכום ההחזר המאושר</div>
                        <div className="text-green text-3xl font-black tracking-tight">₪{app.calculatedRefund?.toLocaleString()}</div>
                      </div>
                      <span className="text-3xl text-green opacity-60">✓</span>
                    </div>
                  )}

                  {isRejected && (
                    <div className="bg-red-l border border-red-b rounded-xl px-4 py-3 flex items-center gap-2 text-red font-semibold text-sm">
                      <span>✕</span> הבקשה נדחתה על ידי הפקיד
                    </div>
                  )}

                  {!isApproved && !isRejected && (
                    <div className="text-sub text-sm">
                      הוגש ב-{new Date(app.createdAt).toLocaleDateString("he-IL")} · הבקשה בטיפול
                    </div>
                  )}
                </Card>
              )}

              {data.history?.length > 0 && (
                <Card className="p-5">
                  <SectionHead title="היסטוריית בקשות" />
                  <div className="flex flex-col gap-2.5">
                    {data.history.map(pa => (
                      <div key={pa.applicationId} className="bg-bg border border-border rounded-xl px-4 py-3 flex justify-between items-center">
                        <div>
                          <div className="text-text font-bold text-sm">שנת {pa.taxYear}</div>
                          <div className="text-sub text-xs mt-0.5">
                            {pa.officerDecisionDate ? new Date(pa.officerDecisionDate).toLocaleDateString("he-IL") : "—"}
                          </div>
                        </div>
                        <div className="flex flex-col items-end gap-1.5">
                          <Badge status={pa.status} forCitizen />
                          {pa.status === "Approved" && (
                            <div className="text-green font-extrabold text-sm">₪{pa.calculatedRefund?.toLocaleString()}</div>
                          )}
                        </div>
                      </div>
                    ))}
                  </div>
                </Card>
              )}
            </>
          );
        })()}
      </div>
    </div>
  );
}