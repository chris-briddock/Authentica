'use client'

import React, { useState, useEffect } from "react";
import { useRecoilState, useRecoilValue } from "recoil";
import Link from "next/link";
import { Mail, Plus, QrCode, Smartphone, Trash2, } from "lucide-react";

import Button from "@/common/components/button/Button";
import { Card } from "@/common/components/card/Card";
import { Switch } from "../switch/Switch";
import { Input } from "@/common/components/input/Input";

import { IUser } from "@/common/models/IUser";
import { userState } from "@/common/state/UserState";
import { isLoggedInState } from "@/common/state/IsLoggedInState";
import { GetAllApplicationsRequest } from "@/features/applications/services/GetAllApplicationsRequest";
import { CreateApplicationRequest } from "@/features/applications/services/CreateApplicationRequest";
import { DeleteApplicationRequest } from "@/features/applications/services/DeleteApplicationRequest";
import { DeviceCodeRequest } from "@/features/auth/services/DeviceCodeRequest";
import { Tabs } from "../tab/Tabs";
import { Tab } from "../tab/Tab";

interface IApplication {
  name: string;
  callbackUri: string;
}

const Dashboard: React.FC = () => {
  const [user, setUser] = useRecoilState<IUser>(userState);
  const isLoggedIn = useRecoilValue(isLoggedInState);
  const [twoFactorEnabled, setTwoFactorEnabled] = useState<boolean>(false);
  const [twoFactorMethod, setTwoFactorMethod] = useState<'email' | 'app'>('email');
  const [activeTab, setActiveTab] = useState<string>('applications');
  // const [showRecoveryCodes, setShowRecoveryCodes] = useState<boolean>(false);
  const [applications, setApplications] = useState<IApplication[]>([]);
  const [newAppName, setNewAppName] = useState<string>('');
  const [newAppCallbackUri, setNewAppCallbackUri] = useState<string>('');
  const [deviceCode, setDeviceCode] = useState<string>('');
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    fetchApplications();
  }, []);

  const fetchApplications = async () => {
    setIsLoading(true);
    setError(null);
    try {
      const response = await GetAllApplicationsRequest();
      if (response.ok) {
        const apps = await response.json();
        setApplications(apps);
      } else {
        setError('Failed to fetch applications');
      }
    } catch (error) {
      setError('Error fetching applications');
      console.error('Error fetching applications:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const handleCreateApplication = async () => {
    setIsLoading(true);
    setError(null);
    try {
      const response = await CreateApplicationRequest({ name: newAppName, callbackUri: newAppCallbackUri });
      if (response.ok) {
        console.log('Application created successfully');
        setNewAppName('');
        setNewAppCallbackUri('');
        fetchApplications();
      } else {
        setError('Failed to create application');
      }
    } catch (error) {
      setError('Error creating application');
      console.error('Error creating application:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const handleDeleteApplication = async (name: string) => {
    setIsLoading(true);
    setError(null);
    try {
      const response = await DeleteApplicationRequest({ name });
      if (response.ok) {
        console.log('Application deleted successfully');
        fetchApplications();
      } else {
        setError('Failed to delete application');
      }
    } catch (error) {
      setError('Error deleting application');
      console.error('Error deleting application:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const handleGenerateDeviceCode = async () => {
    setIsLoading(true);
    setError(null);
    try {
      const response = await DeviceCodeRequest();
      if (response.ok) {
        const data = await response.json();
        setDeviceCode(data.user_code);
      } else {
        setError('Failed to generate device code');
      }
    } catch (error) {
      setError('Error generating device code');
      console.error('Error generating device code:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const handleUpdateDetails = (field: keyof IUser, value: string) => {
    setUser(prevUser => ({ ...prevUser, [field]: value }));
  };

  const handleTwoFactorToggle = () => {
    setTwoFactorEnabled(!twoFactorEnabled);
  };

  // const handleTwoFactorMethodChange = (method: 'email' | 'app') => {
  //   setTwoFactorMethod(method);
  // };

  // const generateRecoveryCodes = (): string[] => {
  //   return Array(8).fill(0).map(() => Math.random().toString(36).substr(2, 8));
  // };

  if (!isLoggedIn) {
    return (
      <div className="bg-gray-800 min-h-screen flex items-center justify-center">
        <div className="container mx-auto p-4 text-white text-center">
          <h1 className="text-2xl font-bold mb-4">
            Please login to view your dashboard.
          </h1>
          <Link 
            href="/login" 
            className="text-pink-500 hover:text-pink-700 inline-block"
          >
            Go to Login
          </Link>
        </div>
      </div>
    );
  }

  return (
    <div className="mx-auto p-4 bg-gray-800 text-gray-300 min-h-screen">
      <div className="container mx-auto">
        <h1 className="text-2xl font-bold mb-4 text-pink-500 text-center">User Dashboard</h1>

        {error && (
          <div className="bg-red-500 text-white p-2 rounded mb-4">
            {error}
          </div>
        )}

        <Tabs activeTab={activeTab} onTabChange={setActiveTab}>
          <Tab value="applications" label="Applications">
            <Card title="Manage Applications">
              <div className="flex flex-col space-y-2">
                <div className="flex items-center space-x-2">
                  <Input
                    value={newAppName}
                    onChange={(e) => setNewAppName(e.target.value)}
                    placeholder="App Name"
                    className="flex-grow"
                  />
                  <Input
                    value={newAppCallbackUri}
                    onChange={(e) => setNewAppCallbackUri(e.target.value)}
                    placeholder="Callback URI"
                    className="flex-grow"
                  />
                  <Button onClick={handleCreateApplication} disabled={isLoading}>
                    <Plus size={18} />
                  </Button>
                </div>
                {isLoading ? (
                  <p>Loading applications...</p>
                ) : applications.length === 0 ? (
                  <p>No applications.</p>
                ) : (
                  <ul className="space-y-2 max-h-60 overflow-y-auto">
                    {applications.map((app, index) => (
                      <li key={index} className="flex justify-between items-center bg-gray-700 p-2 rounded">
                        <span className="truncate flex-grow mr-2">{app.name}</span>
                        <Button onClick={() => handleDeleteApplication(app.name)} variant="danger" disabled={isLoading} className="p-1">
                          <Trash2 size={18} />
                        </Button>
                      </li>
                    ))}
                  </ul>
                )}
              </div>
            </Card>
          </Tab>

          <Tab value="security" label="Security">
            <Card title="Security Settings">
              <div className="space-y-4">
                <div className="flex items-center justify-between">
                  <span>Two-Factor Authentication</span>
                  <Switch
                    checked={twoFactorEnabled}
                    onChange={handleTwoFactorToggle}
                  />
                </div>
                {twoFactorEnabled && (
                  <div className="flex space-x-2">
                    <Button
                      onClick={() => setTwoFactorMethod("email")}
                      variant={twoFactorMethod === "email" ? "primary" : "secondary"}
                      className="flex-1 py-1"
                    >
                      <Mail size={18} className="mr-1" /> Email
                    </Button>
                    <Button
                      onClick={() => setTwoFactorMethod("app")}
                      variant={twoFactorMethod === "app" ? "primary" : "secondary"}
                      className="flex-1 py-1"
                    >
                      <QrCode size={18} className="mr-1" /> App
                    </Button>
                  </div>
                )}
              </div>
            </Card>
          </Tab>

          <Tab value="profile" label="Profile">
            <Card title="Profile Details">
              <div className="space-y-2">
                <Input
                  type="email"
                  value={user.email}
                  onChange={(e) => handleUpdateDetails("email", e.target.value)}
                  placeholder="Email"
                />
                <Input
                  type="tel"
                  value={user.phoneNumber}
                  onChange={(e) => handleUpdateDetails("phoneNumber", e.target.value)}
                  placeholder="Phone Number"
                />
                <Input
                  value={user.address}
                  onChange={(e) => handleUpdateDetails("address", e.target.value)}
                  placeholder="Address"
                />
                <Button onClick={() => console.log("Update profile")} className="w-full">
                  Update Profile
                </Button>
              </div>
            </Card>
          </Tab>

          <Tab value="device" label="Device">
            <Card title="Device Code Generation">
              <div className="space-y-2">
                <Button onClick={handleGenerateDeviceCode} disabled={isLoading} className="w-full">
                  <Smartphone size={18} className="mr-2" /> Generate Code
                </Button>
                {deviceCode && (
                  <div className="bg-gray-700 p-2 rounded-md text-center">
                    <p className="text-lg font-mono">{deviceCode}</p>
                  </div>
                )}
              </div>
            </Card>
          </Tab>
        </Tabs>
      </div>
    </div>
  );
};

export default Dashboard;