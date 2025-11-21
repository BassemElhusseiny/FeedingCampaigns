import React, { useEffect, useState } from 'react'
import api from '../api/client'
import CardStat from '../components/common/CardStat'
import Badge from '../components/common/Badge'

export default function Dashboard() {
  const [loading, setLoading] = useState(true)
  const [campaigns, setCampaigns] = useState([])
  const [beneficiaries, setBeneficiaries] = useState([])
  const [error, setError] = useState('')

  useEffect(() => {
    const load = async () => {
      try {
        const [cRes, bRes] = await Promise.all([
          api.get('/campaigns'),
          api.get('/beneficiaries'),
        ])
        setCampaigns(cRes.data)
        setBeneficiaries(bRes.data)
      } catch (err) {
        console.error(err)
        setError('Failed to load dashboard data.')
      } finally {
        setLoading(false)
      }
    }
    load()
  }, [])

  if (loading) {
    return (
      <div className="p-6 text-slate-200">
        Loading dashboard...
      </div>
    )
  }

  if (error) {
    return (
      <div className="p-6 text-rose-400">
        {error}
      </div>
    )
  }

  const totalCampaigns = campaigns.length
  const activeCampaigns = campaigns.filter(c => c.status === 'Active').length
  const totalMealsTarget = campaigns.reduce((sum, c) => sum + c.targetMeals, 0)
  const totalMealsDistributed = campaigns.reduce((sum, c) => sum + c.mealsDistributed, 0)
  const totalBeneficiaries = beneficiaries.length

  return (
    <div className="p-6 space-y-6">
      <h1 className="text-2xl font-semibold text-slate-50 mb-2">
        Overview
      </h1>

      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
        <CardStat
          label="Total Campaigns"
          value={totalCampaigns}
          subtitle={`${activeCampaigns} active`}
        />
        <CardStat
          label="Total Meals Target"
          value={totalMealsTarget.toLocaleString()}
          subtitle="All campaigns combined"
        />
        <CardStat
          label="Meals Distributed"
          value={totalMealsDistributed.toLocaleString()}
          subtitle="Across all campaigns"
        />
        <CardStat
          label="Registered Beneficiary Families"
          value={totalBeneficiaries}
          subtitle="In the system"
        />
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <div className="bg-slate-900 border border-slate-800 rounded-xl p-4">
          <div className="flex items-center justify-between mb-3">
            <h2 className="text-sm font-semibold text-slate-100">
              Recent Campaigns
            </h2>
          </div>
          <div className="space-y-2">
            {campaigns.slice(0, 5).map(c => (
              <div
                key={c.id}
                className="flex items-center justify-between py-2 border-b border-slate-800/60 last:border-b-0"
              >
                <div>
                  <div className="text-sm font-medium text-slate-100">
                    {c.title}
                  </div>
                  <div className="text-xs text-slate-500">
                    {c.branchName} • {c.categoryName}
                  </div>
                </div>
                <div className="flex flex-col items-end gap-1">
                  <Badge>{c.status}</Badge>
                  <div className="text-xs text-slate-400">
                    {c.mealsDistributed.toLocaleString()} / {c.targetMeals.toLocaleString()} meals
                  </div>
                </div>
              </div>
            ))}
            {campaigns.length === 0 && (
              <div className="text-sm text-slate-500">
                No campaigns found.
              </div>
            )}
          </div>
        </div>

        <div className="bg-slate-900 border border-slate-800 rounded-xl p-4">
          <div className="flex items-center justify-between mb-3">
            <h2 className="text-sm font-semibold text-slate-100">
              Top Vulnerable Families
            </h2>
          </div>
          <div className="space-y-2 max-h-72 overflow-auto pr-1">
            {beneficiaries
              .slice()
              .sort((a, b) => b.vulnerabilityScore - a.vulnerabilityScore)
              .slice(0, 8)
              .map(f => (
                <div
                  key={f.id}
                  className="flex items-center justify-between py-2 border-b border-slate-800/60 last:border-b-0"
                >
                  <div>
                    <div className="text-sm font-medium text-slate-100">
                      {f.headOfFamilyName}
                    </div>
                    <div className="text-xs text-slate-500">
                      {f.familyCode} • {f.area || 'N/A'}
                    </div>
                  </div>
                  <div className="flex flex-col items-end gap-0.5">
                    <span className="text-xs text-slate-400">
                      Members: {f.membersCount}
                    </span>
                    <span className="text-xs font-semibold text-amber-300">
                      Score: {f.vulnerabilityScore}
                    </span>
                  </div>
                </div>
              ))}
            {beneficiaries.length === 0 && (
              <div className="text-sm text-slate-500">
                No families found.
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  )
}
