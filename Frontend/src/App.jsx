import React from 'react'
import { Route, Routes, Navigate } from 'react-router-dom'
import ProtectedRoute from './auth/ProtectedRoute'
import Login from './pages/Login'
import Dashboard from './pages/Dashboard'
import CampaignList from './pages/campaigns/CampaignList'
import CampaignCreate from './pages/campaigns/CampaignCreate'
import CampaignDetails from './pages/campaigns/CampaignDetails'
import BeneficiaryList from './pages/beneficiaries/BeneficiaryList'
import DistributionList from './pages/distributions/DistributionList'
import Sidebar from './components/layout/Sidebar'
import Topbar from './components/layout/Topbar'

function ShellLayout({ children }) {
  return (
    <div className="min-h-screen flex bg-slate-950 text-slate-100">
      <Sidebar />
      <div className="flex-1 flex flex-col">
        <Topbar />
        <main className="flex-1 bg-slate-950">
          {children}
        </main>
      </div>
    </div>
  )
}

export default function App() {
  return (
    <Routes>
      <Route path="/login" element={<Login />} />

      <Route element={<ProtectedRoute />}>
        <Route
          path="/"
          element={<Navigate to="/dashboard" replace />}
        />
        <Route
          path="/dashboard"
          element={
            <ShellLayout>
              <Dashboard />
            </ShellLayout>
          }
        />
        <Route
          path="/campaigns"
          element={
            <ShellLayout>
              <CampaignList />
            </ShellLayout>
          }
        />
        <Route
          path="/campaigns/new"
          element={
            <ShellLayout>
              <CampaignCreate />
            </ShellLayout>
          }
        />
        <Route
          path="/campaigns/:id"
          element={
            <ShellLayout>
              <CampaignDetails />
            </ShellLayout>
          }
        />
        <Route
          path="/beneficiaries"
          element={
            <ShellLayout>
              <BeneficiaryList />
            </ShellLayout>
          }
        />
        <Route
          path="/distributions"
          element={
            <ShellLayout>
              <DistributionList />
            </ShellLayout>
          }
        />
      </Route>

      <Route path="*" element={<Navigate to="/dashboard" replace />} />
    </Routes>
  )
}
