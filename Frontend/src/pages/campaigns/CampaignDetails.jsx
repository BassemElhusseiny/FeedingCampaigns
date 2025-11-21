import React, { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import api from '../../api/client'
import Badge from '../../components/common/Badge'

export default function CampaignDetails() {
  const { id } = useParams()
  const [campaign, setCampaign] = useState(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  useEffect(() => {
    const load = async () => {
      try {
        const res = await api.get(`/campaigns/${id}`)
        setCampaign(res.data)
      } catch (err) {
        console.error(err)
        setError('Failed to load campaign.')
      } finally {
        setLoading(false)
      }
    }
    load()
  }, [id])

  if (loading) {
    return <div className="p-6 text-slate-200">Loading...</div>
  }

  if (error || !campaign) {
    return <div className="p-6 text-rose-400">{error || 'Not found'}</div>
  }

  return (
    <div className="p-6 space-y-4">
      <div className="flex flex-col md:flex-row md:items-center md:justify-between gap-2">
        <div>
          <h1 className="text-2xl font-semibold text-slate-50">
            {campaign.title}
          </h1>
          <p className="text-sm text-slate-400">
            {campaign.branchName} • {campaign.categoryName}
          </p>
        </div>
        <Badge>{campaign.status}</Badge>
      </div>

      <div className="grid md:grid-cols-3 gap-4">
        <div className="bg-slate-900 border border-slate-800 rounded-xl p-4">
          <div className="text-xs text-slate-400 uppercase mb-1">Meals</div>
          <div className="text-lg text-slate-50">
            {campaign.mealsDistributed.toLocaleString()} / {campaign.targetMeals.toLocaleString()}
          </div>
        </div>
        <div className="bg-slate-900 border border-slate-800 rounded-xl p-4">
          <div className="text-xs text-slate-400 uppercase mb-1">Period</div>
          <div className="text-sm text-slate-100">
            {new Date(campaign.startDate).toLocaleDateString()} -{' '}
            {new Date(campaign.endDate).toLocaleDateString()}
          </div>
        </div>
        <div className="bg-slate-900 border border-slate-800 rounded-xl p-4">
          <div className="text-xs text-slate-400 uppercase mb-1">Donations</div>
          <div className="text-sm text-slate-100">
            {campaign.totalDonationsCount} donations, {campaign.totalMonetaryDonations.toLocaleString()} EGP
          </div>
        </div>
      </div>

      <div className="bg-slate-900 border border-slate-800 rounded-xl p-4">
        <div className="text-xs text-slate-400 uppercase mb-2">Description</div>
        <p className="text-sm text-slate-200 whitespace-pre-line">
          {campaign.description}
        </p>
        <div className="text-xs text-slate-500 mt-3">
          Created by {campaign.createdByName}
        </div>
      </div>
    </div>
  )
}
