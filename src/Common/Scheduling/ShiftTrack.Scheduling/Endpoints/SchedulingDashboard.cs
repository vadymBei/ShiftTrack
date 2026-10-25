namespace ShiftTrack.Scheduling.Endpoints;

/// <summary>A self-contained page with the list of jobs and a button to run each of them.</summary>
internal static class SchedulingDashboard
{
    public static string Render(string apiPrefix) => Template.Replace("__API__", apiPrefix);

    private const string Template = """
<!doctype html>
<html lang="uk">
<head>
<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1">
<title>ShiftTrack · Джоби</title>
<style>
  :root { color-scheme: light dark; --bg: #f6f5fa; --card: #fff; --text: #1d1a2b; --muted: #6b6880; --line: #e6e3ef; --accent: #7c3aed; --ok: #15803d; --run: #b45309; --err: #b91c1c; }
  @media (prefers-color-scheme: dark) { :root { --bg: #0e0b17; --card: #1b1729; --text: #f2f0f8; --muted: #a9a4bf; --line: #2c2740; --accent: #8b5cf6; --ok: #86efac; --run: #fcd34d; --err: #fca5a5; } }
  * { box-sizing: border-box; }
  body { margin: 0; padding: 32px 16px; background: var(--bg); color: var(--text); font: 14px/1.5 system-ui, -apple-system, "Segoe UI", sans-serif; }
  main { max-width: 960px; margin: 0 auto; }
  header { display: flex; align-items: center; justify-content: space-between; gap: 16px; margin-bottom: 20px; flex-wrap: wrap; }
  h1 { margin: 0; font-size: 22px; }
  a { color: var(--accent); }
  .card { background: var(--card); border: 1px solid var(--line); border-radius: 14px; overflow-x: auto; }
  table { width: 100%; border-collapse: collapse; }
  th, td { padding: 12px 16px; text-align: left; border-bottom: 1px solid var(--line); white-space: nowrap; }
  th { font-size: 12px; font-weight: 600; color: var(--muted); text-transform: uppercase; letter-spacing: .04em; }
  tr:last-child td { border-bottom: 0; }
  .name { font-weight: 600; }
  .desc { color: var(--muted); font-size: 13px; white-space: normal; }
  .status { font-weight: 600; }
  .idle { color: var(--ok); }
  .running { color: var(--run); }
  button { font: inherit; font-weight: 600; padding: 7px 14px; border-radius: 9px; border: 0; background: var(--accent); color: #fff; cursor: pointer; }
  button:disabled { opacity: .55; cursor: default; }
  .muted { color: var(--muted); }
  .empty, .error { padding: 24px 16px; text-align: center; }
  .error { color: var(--err); }
  #toast { position: fixed; right: 16px; bottom: 16px; padding: 10px 14px; border-radius: 10px; background: var(--card); border: 1px solid var(--line); box-shadow: 0 6px 20px rgba(0,0,0,.15); opacity: 0; transition: opacity .2s; }
  #toast.show { opacity: 1; }
</style>
</head>
<body>
<main>
  <header>
    <h1>Заплановані джоби</h1>
    <span class="muted">Оновлюється кожні 5 с · <a href="swagger">Swagger</a></span>
  </header>
  <div class="card">
    <table>
      <thead><tr><th>Джоба</th><th>Стан</th><th>Попередній запуск</th><th>Наступний запуск</th><th></th></tr></thead>
      <tbody id="jobs"><tr><td colspan="5" class="empty muted">Завантаження…</td></tr></tbody>
    </table>
  </div>
</main>
<div id="toast" role="status"></div>
<script>
  const api = '__API__';
  const body = document.getElementById('jobs');
  const toast = document.getElementById('toast');
  const format = value => value ? new Date(value).toLocaleString('uk-UA') : '—';
  const escape = value => String(value ?? '').replace(/[&<>"']/g, c => ({'&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;'})[c]);

  function notify(text) {
    toast.textContent = text;
    toast.classList.add('show');
    setTimeout(() => toast.classList.remove('show'), 3000);
  }

  async function load() {
    try {
      const response = await fetch(`${api}/jobs`);
      if (!response.ok) throw new Error(response.status);
      const jobs = await response.json();
      body.innerHTML = jobs.length === 0
        ? '<tr><td colspan="5" class="empty muted">Немає зареєстрованих джоб</td></tr>'
        : jobs.map(job => `
          <tr>
            <td><div class="name">${escape(job.name)}</div><div class="desc">${escape(job.description)}</div></td>
            <td class="status ${job.isRunning ? 'running' : 'idle'}">${job.isRunning ? 'Виконується' : 'Очікує'}</td>
            <td>${format(job.previousFireTime)}</td>
            <td>${format(job.nextFireTime)}</td>
            <td><button data-job="${escape(job.name)}" ${job.isRunning ? 'disabled' : ''}>Запустити</button></td>
          </tr>`).join('');
    } catch {
      body.innerHTML = '<tr><td colspan="5" class="error">Не вдалося завантажити список джоб</td></tr>';
    }
  }

  body.addEventListener('click', async event => {
    const button = event.target.closest('button[data-job]');
    if (!button) return;
    button.disabled = true;
    const name = button.dataset.job;
    const response = await fetch(`${api}/jobs/${encodeURIComponent(name)}/trigger`, {method: 'POST'});
    notify(response.ok ? `${name} запущено` : `Не вдалося запустити ${name}`);
    setTimeout(load, 500);
  });

  load();
  setInterval(load, 5000);
</script>
</body>
</html>
""";
}
