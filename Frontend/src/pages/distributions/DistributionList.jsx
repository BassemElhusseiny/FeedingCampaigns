import React, { useEffect, useState } from 'react'
import api from '../../api/client'

export default function DistributionList() {
  const [campaignId, setCampaignId] = useState('')
  const [campaigns, setCampaigns] = useState([])
  const [batches, setBatches] = useState([])
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState('')

  useEffect(() => {
    const loadCampaigns = async () => {
      try {
        const res = await api.get('/campaigns')
        setCampaigns(res.data)
      } catch (err) {
        console.error(err)
      }
    }
    loadCampaigns()
  }, [])

  const loadBatches = async (id) => {
    if (!id) return
    setLoading(true)
    setError('')
    try {
      const res = await api.get(`/distributions/by-campaign/${id}`)
      setBatches(res.data)
    } catch (err) {
      console.error(err)
      setError('Failed to load distributions.')
    } finally {
      setLoading(false)
    }
  }

  const handleChange = async (e) => {
    const id = e.target.value
    setCampaignId(id)
    await loadBatches(id)
  }

  return (
    <div className="p-6 space-y-4">
      <div className="flex flex-col md:flex-row md:items-center md:justify-between gap-3">
        <div>
          <h1 className="text-2xl font-semibold text-slate-50">
            Distributions
          </h1>
          <p className="text-sm text-slate-400">
            View distribution batches per campaign.
          </p>
        </div>
      </div>

      <div className="bg-slate-900 border border-slate-800 rounded-xl p-4 space-y-4">
        <div className="flex gap-3 items-center">
          <label className="text-xs text-slate-300">
            Campaign
          </label>
          <select
            value={campaignId}
            onChange={handleChange}
            className="rounded-lg bg-slate-950 border border-slate-700 px-3 py-1.5 text-xs text-slate-100"
          >
            <option value="">Select a campaign...</option>
            {campaigns.map(c => (
              <option key={c.id} value={c.id}>
                {c.title}
              </option>
            ))}
          </select>
        </div>

        {loading ? (
          <div className="text-slate-300 text-sm">Loading distributions...</div>
        ) : error ? (
          <div className="text-rose-400 text-sm">{error}</div>
        ) : campaignId && batches.length === 0 ? (
          <div className="text-slate-400 text-sm">No distributions found.</div>
        ) : (
          campaignId && (
            <div className="overflow-auto">
              <table className="min-w-full text-sm">
                <thead>
                  <tr className="border-b border-slate-800 text-xs text-slate-400 uppercase">
                    <th className="text-left py-2 pr-4">Date</th>
                    <th className="text-left py-2 pr-4">Total meals</th>
                    <th className="text-left py-2 pr-4">Families served</th>
                  </tr>
                </thead>
                <tbody>
                  {batches.map(b => (
                    <tr
                      key={b.id}
                      className="border-b border-slate-800/60 last:border-b-0 hover:bg-slate-800/40"
                    >
                      <td className="py-2 pr-4 text-slate-100">
                        {new Date(b.distributionDate).toLocaleString()}
                      </td>
                      <td className="py-2 pr-4 text-slate-300 text-xs">
                        {b.totalMealsDelivered.toLocaleString()}
                      </td>
                      <td className="py-2 pr-4 text-slate-300 text-xs">
                        {b.familiesServed}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )
        )}
      </div>
    </div>
  )
}
