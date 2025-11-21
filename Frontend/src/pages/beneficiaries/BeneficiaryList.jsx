import React, { useEffect, useState } from 'react'
import api from '../../api/client'

export default function BeneficiaryList() {
  const [families, setFamilies] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [search, setSearch] = useState('')

  useEffect(() => {
    const load = async () => {
      try {
        const res = await api.get('/beneficiaries')
        setFamilies(res.data)
      } catch (err) {
        console.error(err)
        setError('Failed to load families.')
      } finally {
        setLoading(false)
      }
    }
    load()
  }, [])

  const filtered = families.filter(f => {
    const term = search.toLowerCase()
    return (
      !term ||
      f.familyCode.toLowerCase().includes(term) ||
      f.headOfFamilyName.toLowerCase().includes(term) ||
      (f.area || '').toLowerCase().includes(term)
    )
  })

  return (
    <div className="p-6 space-y-4">
      <div className="flex flex-col md:flex-row md:items-center md:justify-between gap-3">
        <div>
          <h1 className="text-2xl font-semibold text-slate-50">
            Beneficiary Families
          </h1>
          <p className="text-sm text-slate-400">
            Registered families and their vulnerability scores.
          </p>
        </div>
      </div>

      <div className="bg-slate-900 border border-slate-800 rounded-xl p-4 space-y-4">
        <div className="flex justify-between gap-3">
          <input
            type="text"
            placeholder="Search by code, name, or area..."
            className="w-full md:max-w-sm rounded-lg bg-slate-950 border border-slate-700 px-3 py-1.5 text-xs text-slate-100"
            value={search}
            onChange={(e) => setSearch(e.target.value)}
          />
        </div>

        {loading ? (
          <div className="text-slate-300 text-sm">Loading families...</div>
        ) : error ? (
          <div className="text-rose-400 text-sm">{error}</div>
        ) : filtered.length === 0 ? (
          <div className="text-slate-400 text-sm">No families found.</div>
        ) : (
          <div className="overflow-auto">
            <table className="min-w-full text-sm">
              <thead>
                <tr className="border-b border-slate-800 text-xs text-slate-400 uppercase">
                  <th className="text-left py-2 pr-4">Code</th>
                  <th className="text-left py-2 pr-4">Head of family</th>
                  <th className="text-left py-2 pr-4">Area</th>
                  <th className="text-left py-2 pr-4">Phone</th>
                  <th className="text-left py-2 pr-4">Members</th>
                  <th className="text-left py-2 pr-4">Score</th>
                </tr>
              </thead>
              <tbody>
                {filtered.map(f => (
                  <tr
                    key={f.id}
                    className="border-b border-slate-800/60 last:border-b-0 hover:bg-slate-800/40"
                  >
                    <td className="py-2 pr-4 text-slate-100">
                      {f.familyCode}
                    </td>
                    <td className="py-2 pr-4 text-slate-300">
                      {f.headOfFamilyName}
                    </td>
                    <td className="py-2 pr-4 text-slate-300">
                      {f.area || 'N/A'}
                    </td>
                    <td className="py-2 pr-4 text-slate-300 text-xs">
                      {f.phoneNumber || '—'}
                    </td>
                    <td className="py-2 pr-4 text-slate-300 text-xs">
                      {f.membersCount}
                    </td>
                    <td className="py-2 pr-4 text-amber-300 text-xs">
                      {f.vulnerabilityScore}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </div>
  )
}
