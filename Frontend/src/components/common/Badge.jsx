import React from 'react'

const colors = {
  Active: 'bg-emerald-500/10 text-emerald-300 border-emerald-500/40',
  Planned: 'bg-sky-500/10 text-sky-300 border-sky-500/40',
  Completed: 'bg-slate-500/10 text-slate-300 border-slate-500/40',
  Draft: 'bg-slate-700/40 text-slate-200 border-slate-500/40',
  Paused: 'bg-amber-500/10 text-amber-300 border-amber-500/40',
  Cancelled: 'bg-rose-500/10 text-rose-300 border-rose-500/40',
}

export default function Badge({ children }) {
  const cls = colors[children] || 'bg-slate-700/40 text-slate-200 border-slate-500/40'
  return (
    <span className={`inline-flex items-center px-2 py-0.5 rounded-full text-xs border ${cls}`}>
      {children}
    </span>
  )
}
