import React from 'react'

export default function CardStat({ label, value, subtitle }) {
  return (
    <div className="bg-slate-900 border border-slate-800 rounded-xl p-4 flex flex-col gap-1">
      <div className="text-xs uppercase tracking-wide text-slate-400">
        {label}
      </div>
      <div className="text-2xl font-semibold text-slate-50">
        {value}
      </div>
      {subtitle && (
        <div className="text-xs text-slate-500">
          {subtitle}
        </div>
      )}
    </div>
  )
}
