import React, { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import api from '../../api/client'
import { useAuth } from '../../auth/AuthContext'

export default function CampaignCreate() {
  const navigate = useNavigate()
  const { user } = useAuth()
  const [branches, setBranches] = useState([])
  const [categories, setCategories] = useState([])
  const [form, setForm] = useState({
    title: '',
    description: '',
    branchId: '',
    categoryId: '',
    startDate: '',
    endDate: '',
    targetMeals: 100,
  })
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState('')

  useEffect(() => {
    const load = async () => {
      try {
        const [campsRes] = await Promise.all([
          api.get('/campaigns'),
        ])
        const branchesSet = new Map()
        const categoriesSet = new Map()
        campsRes.data.forEach(c => {
          branchesSet.set(c.branchName, c.branchName)
          categoriesSet.set(c.categoryName, c.categoryName)
        })
        setBranches(Array.from(branchesSet.keys()).map((name, idx) => ({ id: idx + 1, name })))
        setCategories(Array.from(categoriesSet.keys()).map((name, idx) => ({ id: idx + 1, name })))
      } catch (err) {
        console.error(err)
      }
    }
    load()
  }, [])

  const handleChange = (e) => {
    const { name, value } = e.target
    setForm(prev => ({ ...prev, [name]: value }))
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    setError('')
    setLoading(true)
    try {
      const payload = {
        title: form.title,
        description: form.description,
        branchId: Number(form.branchId) || 1,
        categoryId: Number(form.categoryId) || 1,
        startDate: form.startDate,
        endDate: form.endDate,
        targetMeals: Number(form.targetMeals),
        createdByUserId: user?.userId,
      }
      await api.post('/campaigns', payload)
      navigate('/campaigns')
    } catch (err) {
      console.error(err)
      setError('Failed to create campaign.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="p-6 max-w-3xl">
      <h1 className="text-2xl font-semibold text-slate-50 mb-2">
        New Campaign
      </h1>
      <p className="text-sm text-slate-400 mb-6">
        Create a new feeding campaign for a specific branch and period.
      </p>

      <form onSubmit={handleSubmit} className="bg-slate-900 border border-slate-800 rounded-xl p-5 space-y-4">
        <div className="grid md:grid-cols-2 gap-4">
          <div>
            <label className="block text-xs text-slate-300 mb-1">Title</label>
            <input
              type="text"
              name="title"
              className="w-full rounded-lg bg-slate-950 border border-slate-700 px-3 py-1.5 text-sm text-slate-100"
              value={form.title}
              onChange={handleChange}
              required
            />
          </div>
          <div>
            <label className="block text-xs text-slate-300 mb-1">Target meals</label>
            <input
              type="number"
              name="targetMeals"
              min="1"
              className="w-full rounded-lg bg-slate-950 border border-slate-700 px-3 py-1.5 text-sm text-slate-100"
              value={form.targetMeals}
              onChange={handleChange}
              required
            />
          </div>
        </div>

        <div>
          <label className="block text-xs text-slate-300 mb-1">Description</label>
          <textarea
            name="description"
            rows="3"
            className="w-full rounded-lg bg-slate-950 border border-slate-700 px-3 py-2 text-sm text-slate-100"
            value={form.description}
            onChange={handleChange}
            required
          />
        </div>

        <div className="grid md:grid-cols-2 gap-4">
          <div>
            <label className="block text-xs text-slate-300 mb-1">Start date</label>
            <input
              type="date"
              name="startDate"
              className="w-full rounded-lg bg-slate-950 border border-slate-700 px-3 py-1.5 text-sm text-slate-100"
              value={form.startDate}
              onChange={handleChange}
              required
            />
          </div>
          <div>
            <label className="block text-xs text-slate-300 mb-1">End date</label>
            <input
              type="date"
              name="endDate"
              className="w-full rounded-lg bg-slate-950 border border-slate-700 px-3 py-1.5 text-sm text-slate-100"
              value={form.endDate}
              onChange={handleChange}
              required
            />
          </div>
        </div>

        <div className="flex gap-4">
          <div className="flex-1">
            <label className="block text-xs text-slate-300 mb-1">Branch</label>
            <select
              name="branchId"
              className="w-full rounded-lg bg-slate-950 border border-slate-700 px-3 py-1.5 text-sm text-slate-100"
              value={form.branchId}
              onChange={handleChange}
            >
              <option value="">Default branch</option>
              {branches.map(b => (
                <option key={b.id} value={b.id}>{b.name}</option>
              ))}
            </select>
          </div>
          <div className="flex-1">
            <label className="block text-xs text-slate-300 mb-1">Category</label>
            <select
              name="categoryId"
              className="w-full rounded-lg bg-slate-950 border border-slate-700 px-3 py-1.5 text-sm text-slate-100"
              value={form.categoryId}
              onChange={handleChange}
            >
              <option value="">Default category</option>
              {categories.map(c => (
                <option key={c.id} value={c.id}>{c.name}</option>
              ))}
            </select>
          </div>
        </div>

        {error && (
          <div className="text-sm text-rose-400">
            {error}
          </div>
        )}

        <div className="flex justify-end gap-3 pt-2">
          <button
            type="button"
            onClick={() => navigate(-1)}
            className="px-4 py-2 rounded-lg text-xs font-medium bg-slate-800 text-slate-200 hover:bg-slate-700"
          >
            Cancel
          </button>
          <button
            type="submit"
            disabled={loading}
            className="px-4 py-2 rounded-lg text-xs font-medium bg-primary-600 text-white hover:bg-primary-500 disabled:opacity-60"
          >
            {loading ? 'Creating...' : 'Create campaign'}
          </button>
        </div>
      </form>
    </div>
  )
}
