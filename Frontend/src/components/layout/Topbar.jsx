import React from 'react'
import { useAuth } from '../../auth/AuthContext'

export default function Topbar() {
  const { user, logout } = useAuth()

  return (
    <header className="h-16 border-b border-slate-800 bg-slate-950/70 backdrop-blur flex items-center justify-between px-6">
      <div className="text-slate-200 font-semibold text-lg">Admin Dashboard</div>
      <div className="flex items-center gap-4">
        <span className="text-sm text-slate-300">
          {user?.fullName}
        </span>
        <button
          onClick={logout}
          className="px-3 py-1.5 rounded-md text-xs font-medium bg-slate-800 text-slate-200 hover:bg-slate-700"
        >
          Logout
        </button>
      </div>
    </header>
  )
}
