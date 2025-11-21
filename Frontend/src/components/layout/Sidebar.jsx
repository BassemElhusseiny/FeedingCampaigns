import React from 'react'
import { NavLink } from 'react-router-dom'
import { useAuth } from '../../auth/AuthContext'

const navItems = [
  { to: '/dashboard', label: 'Dashboard' },
  { to: '/campaigns', label: 'Campaigns' },
  { to: '/beneficiaries', label: 'Beneficiaries' },
  { to: '/distributions', label: 'Distributions' },
]

export default function Sidebar() {
  const { user } = useAuth()

  return (
    <aside className="w-64 bg-slate-950 border-r border-slate-800 flex flex-col">
      <div className="px-4 py-5 border-b border-slate-800">
        <div className="text-primary-400 font-semibold text-lg">
          Feeding Campaigns
        </div>
        <div className="text-xs text-slate-400 mt-1">
          {user?.role} • {user?.email}
        </div>
      </div>
      <nav className="flex-1 px-2 py-4 space-y-1">
        {navItems.map((item) => (
          <NavLink
            key={item.to}
            to={item.to}
            className={({ isActive }) =>
              [
                'block px-3 py-2 rounded-md text-sm font-medium',
                isActive
                  ? 'bg-primary-600 text-white'
                  : 'text-slate-300 hover:bg-slate-800 hover:text-white',
              ].join(' ')
            }
          >
            {item.label}
          </NavLink>
        ))}
      </nav>
      <div className="px-4 py-4 border-t border-slate-800 text-xs text-slate-500">
        &copy; {new Date().getFullYear()} Feeding NGO
      </div>
    </aside>
  )
}
