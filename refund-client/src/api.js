const BASE = import.meta.env.VITE_API_URL ?? "http://localhost:8081";

async function handleResponse(res) {
  if (!res.ok) {
    const text = await res.text();
    try {
      const json = JSON.parse(text);
      throw new Error(json.message ?? text);
    } catch {
      throw new Error(text);
    }
  }
  return res.json();
}

export async function getPendingApplications() {
  return handleResponse(await fetch(`${BASE}/api/applications/pending`));
}

export async function getApplicationDetail(id) {
  return handleResponse(await fetch(`${BASE}/api/applications/${id}`));
}

export async function calculateRefund(id) {
  return handleResponse(await fetch(`${BASE}/api/applications/${id}/calculate`, { method: "POST" }));
}

export async function decideApplication(id, isApproved) {
  return handleResponse(await fetch(`${BASE}/api/applications/${id}/decide`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ isApproved }),
  }));
}

export async function getCitizenHistory(identityNumber) {
  return handleResponse(await fetch(`${BASE}/api/citizens/${identityNumber}/history`));
}