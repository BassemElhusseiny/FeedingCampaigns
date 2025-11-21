import React, { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import api from '../../api/client'
import Badge from '../../components/common/Badge'

export default function CampaignList() {
  const [campaigns, setCampaigns] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [statusFilter, setStatusFilter] = useState('All')
  const [search, setSearch] = useState('')

  useEffect(() => {
    const load = async () => {
      try {
        const res = await api.get('/campaigns')
        setCampaigns(res.data)
      } catch (err) {
        console.error(err)
        setError('Failed to load campaigns.')
      } finally {
        setLoading(false)
      }
    }
    load()
  }, [])

  const filtered = campaigns.filter(c => {
    const matchesStatus = statusFilter === 'All' || c.status === statusFilter
    const matchesSearch =
      !search ||
      c.title.toLowerCase().includes(search.toLowerCase()) ||
      c.branchName.toLowerCase().includes(search.toLowerCase())
    return matchesStatus && matchesSearch
  })

  return (
    <div className="p-6 space-y-4">
      <div className="flex flex-col md:flex-row md:items-center md:justify-between gap-3">
        <div>
          <h1 className="text-2xl font-semibold text-slate-50">
            Campaigns
          </h1>
          <p className="text-sm text-slate-400">
            Manage and monitor all feeding campaigns.
          </p>
        </div>
        <div className="flex gap-2">
          <Link
            to="/campaigns/new"
            className="inline-flex items-center px-4 py-2 rounded-lg bg-primary-600 hover:bg-primary-500 text-sm font-medium text-white"
          >
            + New Campaign
          </Link>
        </div>
      </div>

      <div className="bg-slate-900 border border-slate-800 rounded-xl p-4 space-y-4">
        <div className="flex flex-col md:flex-row gap-3 md:items-center md:justify-between">
          <div className="flex gap-2">
            <select
              value={statusFilter}
              onChange={(e) => setStatusFilter(e.target.value)}
              className="rounded-lg bg-slate-950 border border-slate-700 px-3 py-1.5 text-xs text-slate-100 focus:outline-none focus:ring-1 focus:ring-primary-500"
            >
              <option value="All">All statuses</option>
              <option value="Planned">Planned</option>
              <option value="Active">Active</option>
              <option value="Completed">Completed</option>
              <option value="Draft">Draft</option>
              <option value="Paused">Paused</option>
              <option value="Cancelled">Cancelled</option>
            </select>
          </div>
          <div className="flex-1 md:max-w-xs">
            <input
              type="text"
              placeholder="Search by title or branch..."
              className="w-full rounded-lg bg-slate-950 border border-slate-700 px-3 py-1.5 text-xs text-slate-100 focus:outline-none focus:ring-1 focus:ring-primary-500"
              value={search}
              onChange={(e) => setSearch(e.target.value)}
            />
          </div>
        </div>

        {loading ? (
          <div className="text-slate-300 text-sm">Loading campaigns...</div>
        ) : error ? (
          <div className="text-rose-400 text-sm">{error}</div>
        ) : filtered.length === 0 ? (
          <div className="text-slate-400 text-sm">No campaigns found.</div>
        ) : (
          <div className="overflow-auto">
            <table className="min-w-full text-sm">
              <thead>
                <tr className="border-b border-slate-800 text-xs text-slate-400 uppercase">
                  <th className="text-left py-2 pr-4">Title</th>
                  <th className="text-left py-2 pr-4">Branch</th>
                  <th className="text-left py-2 pr-4">Category</th>
                  <th className="text-left py-2 pr-4">Period</th>
                  <th className="text-left py-2 pr-4">Meals</th>
                  <th className="text-left py-2 pr-4">Status</th>
                  <th className="text-right py-2 pl-4">Actions</th>
                </tr>
              </thead>
              <tbody>
                {filtered.map(c => (
                  <tr
                    key={c.id}
                    className="border-b border-slate-800/60 last:border-b-0 hover:bg-slate-800/40"
                  >
                    <td className="py-2 pr-4 text-slate-100">
                      {c.title}
                    </td>
                    <td className="py-2 pr-4 text-slate-300">
                      {c.branchName}
                    </td>
                    <td className="py-2 pr-4 text-slate-300">
                      {c.categoryName}
                    </td>
                    <td className="py-2 pr-4 text-slate-400 text-xs">
                      {new Date(c.startDate).toLocaleDateString()} -{' '}
                      {new Date(c.endDate).toLocaleDateString()}
                    </td>
                    <td className="py-2 pr-4 text-slate-300 text-xs">
                      {c.mealsDistributed.toLocaleString()} /{' '}
                      {c.targetMeals.toLocaleString()}
                    </td>
                    <td className="py-2 pr-4">
                      <Badge>{c.status}</Badge>
                    </td>
                    <td className="py-2 pl-4 text-right">
                      <Link
                        to={`/campaigns/${c.id}`}
                        className="text-xs text-primary-400 hover:text-primary-300"
                      >
                        View details
                      </Link>
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
