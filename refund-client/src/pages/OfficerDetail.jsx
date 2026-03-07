import { useState, useEffect } from "react";
import { MONTHS } from "../constants/config";
import { getApplicationDetail, calculateRefund, decideApplication } from "../api";
import Avatar from "../components/Avatar";
import Badge from "../components/Badge";
import Card from "../components/Card";
import SectionHead from "../components/SectionHead";
import Spinner from "../components/Spinner";
import ErrorMsg from "../components/ErrorMsg";
import CalcWarning from "../components/CalcWarning";

export default function OfficerDetail({ applicationId, onBack }) {
  const [data, setData]          = useState(null);
  const [loading, setLoading]    = useState(true);
  const [error, setError]        = useState(null);
  const [stage, setStage]        = useState("idle"); // idle | calcError | calculated | decided
  const [calcData, setCalc]      = useState(null);
  const [calcErrMsg, setCalcErr] = useState(null);
  const [decision, setDec]       = useState(null);
  const [decideError, setDecErr] = useState(null);
  const [openYear, setOpenYr]    = useState(null);

  async function load() {
    setLoading(true); setError(null);
    try { setData(await getApplicationDetail(applicationId)); }
    catch (e) { setError(e.message); }
    finally { setLoading(false); }
  }

  useEffect(() => { load(); }, [applicationId]);

  async function handleCalculate() {
    setCalcErr(null);
    try {
      const result = await calculateRefund(applicationId);
      setCalc(result);
      setStage("calculated");
      setData(await getApplicationDetail(applicationId));
    } catch (e) {
      setCalcErr(e.message);
      setStage("calcError");
    }
  }

  async function handleDecide(isApproved) {
    setDecErr(null);
    try {
      await decideApplication(applicationId, isApproved);
      setDec(isApproved);
      setStage("decided");
      setData(await getApplicationDetail(applicationId));
    } catch (e) {
      setDecErr(e.message);
    }
  }

  if (loading) return <div className="p-10" dir="rtl"><Spinner /></div>;
  if (error)   return <div className="p-10" dir="rtl"><ErrorMsg message={error} onRetry={load} /></div>;
  if (!data)   return null;

  const d = data;

  return (
    <div className="px-7 pt-8 pb-16" dir="rtl">
      <div className="max-w-5xl mx-auto">

        <button
          onClick={onBack}
          className="mb-6 px-4 py-1.5 text-sm text-sub border border-border rounded-lg bg-surf shadow-sm hover:text-accent hover:border-accent transition-colors"
        >
          ← חזרה
        </button>

        {/* Hero */}
        <Card className="p-5 mb-3.5 flex items-center gap-4">
          <Avatar name={d.citizen.fullName} size={50} />
          <div className="flex-1">
            <div className="text-text font-extrabold text-lg">{d.citizen.fullName}</div>
            <div className="text-sub text-xs font-mono mt-0.5">ת.ז. {d.citizen.identityNumber}</div>
          </div>
          <div className="bg-accent-l text-accent px-4 py-1.5 rounded-lg text-sm font-bold">
            שנת מס {d.currentApplication.taxYear}
          </div>
        </Card>

        <div className="grid grid-cols-[1fr_300px] gap-3.5 mb-3.5">

          {/* Current application */}
          <Card className="p-5">
            <SectionHead title="בקשה נוכחית" />

            <div className="grid grid-cols-3 gap-2.5 mb-5">
              {[
                { label: "שנת מס",      node: <span className="text-accent text-2xl font-black">{d.currentApplication.taxYear}</span> },
                { label: "ממוצע הכנסה", node: <span className="text-text text-base font-bold">{d.currentApplication.averageIncome ? `₪${d.currentApplication.averageIncome.toLocaleString()}` : "—"}</span> },
                { label: "סטטוס",       node: <div className="mt-0.5"><Badge status={d.currentApplication.status} /></div> },
              ].map((f, i) => (
                <div key={i} className="bg-bg border border-border rounded-xl p-3">
                  <div className="text-sub text-[10px] font-bold tracking-widest uppercase mb-2">{f.label}</div>
                  {f.node}
                </div>
              ))}
            </div>

            {decideError && <div className="mb-3"><ErrorMsg message={decideError} /></div>}

            {stage === "idle" && (
              <button
                onClick={handleCalculate}
                className="w-full py-3.5 rounded-xl bg-accent text-white font-bold text-sm hover:opacity-90 transition-opacity shadow-md"
              >
                חשב זכאות
              </button>
            )}

            {stage === "calcError" && (
              <CalcWarning message={calcErrMsg} onReject={() => handleDecide(false)} />
            )}

            {stage === "calculated" && calcData && (
              <>
                <div className="bg-purple-l border border-purple-b rounded-xl p-4 mb-3 flex justify-between items-center">
                  <div>
                    <div className="text-sub text-[10px] font-bold tracking-widest uppercase mb-1">סכום זכאות</div>
                    <div className="text-purple text-3xl font-black tracking-tight">₪{calcData.calculatedRefund?.toLocaleString()}</div>
                  </div>
                  <div className="text-left">
                    <div className="text-sub text-[10px] font-bold tracking-widest uppercase mb-1">תקציב זמין</div>
                    <div className="text-green text-3xl font-black tracking-tight">₪{calcData.currentBudget?.toLocaleString()}</div>
                  </div>
                </div>
                <div className="grid grid-cols-2 gap-2.5">
                  <button
                    onClick={() => handleDecide(false)}
                    className="py-3 rounded-xl border border-red-b bg-red-l text-red font-bold text-sm hover:bg-red hover:text-white hover:border-red transition-colors"
                  >
                    ✕  דחה
                  </button>
                  <button
                    onClick={() => handleDecide(true)}
                    className="py-3 rounded-xl border border-green-b bg-green-l text-green font-bold text-sm hover:bg-green hover:text-white hover:border-green transition-colors"
                  >
                    ✓  אשר
                  </button>
                </div>
              </>
            )}

            {stage === "decided" && (
              <div className={`rounded-xl p-4 flex items-center gap-3 ${decision ? "bg-green-l border border-green-b" : "bg-red-l border border-red-b"}`}>
                <div className={`w-8 h-8 rounded-full flex items-center justify-center text-white font-black text-sm shrink-0 ${decision ? "bg-green" : "bg-red"}`}>
                  {decision ? "✓" : "✕"}
                </div>
                <div>
                  <div className={`font-extrabold text-sm ${decision ? "text-green" : "text-red"}`}>
                    {decision ? "הבקשה אושרה בהצלחה" : "הבקשה נדחתה"}
                  </div>
                  {decision && calcData && (
                    <div className="text-sub text-xs mt-0.5">₪{calcData.calculatedRefund?.toLocaleString()} הופחת מהתקציב</div>
                  )}
                </div>
              </div>
            )}
          </Card>

          {/* History */}
          <Card className="p-5">
            <SectionHead title="היסטוריית בקשות" />
            {d.pastApplications.length === 0
              ? <div className="text-sub text-sm text-center pt-3">אין היסטוריה</div>
              : (
                <div className="flex flex-col gap-2.5">
                  {d.pastApplications.map(app => (
                    <div key={app.applicationId} className="bg-bg border border-border rounded-xl p-3.5">
                      <div className="flex justify-between items-center mb-1.5">
                        <span className="text-text font-bold text-sm">{app.taxYear}</span>
                        <Badge status={app.status} />
                      </div>
                      {app.status === "Approved" && (
                        <div className="text-green font-extrabold text-lg mb-1">
                          ₪{app.calculatedRefund?.toLocaleString()}
                        </div>
                      )}
                      <div className="text-sub text-xs">
                        {app.officerDecisionDate ? new Date(app.officerDecisionDate).toLocaleDateString("he-IL") : "—"}
                      </div>
                    </div>
                  ))}
                </div>
              )
            }
          </Card>
        </div>

        {/* Incomes */}
        <Card className="p-5">
          <SectionHead title="נתוני הכנסות" />
          <div className="flex flex-col gap-2">
            {d.incomes.map(y => (
              <div key={y.year} className="border border-border rounded-xl overflow-hidden">
                <button
                  onClick={() => setOpenYr(openYear === y.year ? null : y.year)}
                  className="w-full bg-bg px-4 py-3 flex justify-between items-center hover:bg-border/30 transition-colors"
                >
                  <div className="flex items-center gap-2.5">
                    <span className="text-text font-bold text-sm">{y.year}</span>
                    <span className={`text-xs ${y.months.length < 6 ? "text-red font-bold" : "text-sub"}`}>
                      {y.months.length} חודשים {y.months.length < 6 ? "· פחות מ-6 ⚠" : ""}
                    </span>
                  </div>
                </button>
                {openYear === y.year && (
                  <div className="px-4 pt-3 pb-4 grid grid-cols-6 gap-2 border-t border-border">
                    {y.months.map(m => (
                      <div key={m.month} className="bg-bg border border-border rounded-lg py-2 px-1 text-center">
                        <div className="text-sub text-[9px] mb-1">{MONTHS[m.month - 1]}</div>
                        <div className="text-text text-xs font-bold">₪{m.amount?.toLocaleString()}</div>
                      </div>
                    ))}
                  </div>
                )}
              </div>
            ))}
          </div>
        </Card>
      </div>
    </div>
  );
}