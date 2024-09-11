'use client'

import React, { useState, FormEvent, ChangeEvent } from 'react';
import { User, Lock } from 'lucide-react';
import Link from 'next/link';
import { useRouter } from 'next/navigation';
import { TokenRequest } from '@/features/auth/services/TokenRequest';
import { ILoginData } from "@/features/auth/requests/ILoginRequest";
import { LoginRequest } from '../services/LoginRequest';
import { useSetRecoilState } from 'recoil';
import { isLoggedInState } from '@/common/state/IsLoggedInState';

const LoginForm: React.FC = () => {
  const [email, setEmail] = useState<string>('');
  const [password, setPassword] = useState<string>('');
  const [rememberMe, setRememberMe] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const setIsLoggedIn = useSetRecoilState(isLoggedInState);
  const router = useRouter();

  const onSubmit = async () => {
    setError(null);
    setIsLoading(true);

    try {
      const loginData: ILoginData = { email, password, rememberMe };
      const loginResponse = await LoginRequest(loginData);
      

      if (!loginResponse.ok) {
        throw new Error('Login failed');
      }

      const tokenResponse = await TokenRequest();

      if (!tokenResponse.ok) {
        const errorData = await tokenResponse.json();
        throw new Error(errorData.error_description || 'Failed to obtain access token');
      }
      const { accessToken, refreshToken } = await tokenResponse.json();
      
      if (tokenResponse.ok) {
        setIsLoggedIn(true);
        localStorage.setItem('accessToken', accessToken);
        localStorage.setItem('refreshToken', refreshToken);
        router.push('/dashboard');
      }
      
    } catch (error) {
      console.error('Login error:', error);
      setError(error instanceof Error ? error.message : 'An unexpected error occurred');
    } finally {
      setIsLoading(false);
    }
  }

  const handleSubmit = (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    onSubmit();
  };

  const handleEmailChange = (e: ChangeEvent<HTMLInputElement>) => {
    setEmail(e.target.value);
  };

  const handlePasswordChange = (e: ChangeEvent<HTMLInputElement>) => {
    setPassword(e.target.value);
  };

  const handleRememberMeChange = (e: ChangeEvent<HTMLInputElement>) => {
    setRememberMe(e.target.checked);
  };

  return (
    <div className="flex items-center justify-center min-h-screen bg-gray-900">
      <div className="w-full max-w-md">
        <form onSubmit={handleSubmit} className="bg-gray-800 shadow-md rounded px-8 pt-6 pb-8 mb-4">
          {error && (
            <div className="mb-4 bg-red-500 text-white p-3 rounded">
              {error}
            </div>
          )}
          <div className="mb-4 relative">
            <div className="absolute inset-y-0 left-0 pl-6 flex items-center pointer-events-none">
              <User className="h-5 w-5 text-gray-500" />
            </div>
            <input
              className="bg-gray-700 appearance-none border-0 rounded w-full py-3 px-6 pl-14 text-gray-300 leading-tight focus:outline-none focus:ring-2 focus:ring-pink-500 focus:ring-opacity-50"
              id="email"
              type="text"
              placeholder="Email"
              value={email}
              onChange={handleEmailChange}
              required
            />
          </div>
          <div className="mb-6 relative">
            <div className="absolute inset-y-0 left-0 pl-6 flex items-center pointer-events-none">
              <Lock className="h-5 w-5 text-gray-500" />
            </div>
            <input
              className="bg-gray-700 appearance-none border-0 rounded w-full py-3 px-6 pl-14 text-gray-300 leading-tight focus:outline-none focus:ring-2 focus:ring-pink-500 focus:ring-opacity-50"
              id="password"
              type="password"
              placeholder="Password"
              value={password}
              onChange={handlePasswordChange}
              required
            />
          </div>
          <div className="mb-6 flex items-center">
            <input
              id="remember-me"
              type="checkbox"
              className="h-4 w-4 text-pink-600 focus:ring-pink-500 border-gray-300 rounded"
              checked={rememberMe}
              onChange={handleRememberMeChange}
            />
            <label htmlFor="remember-me" className="ml-2 block text-sm text-gray-300">
              Remember me
            </label>
          </div>
          <div className="flex items-center justify-between">
            <button
              className="bg-pink-500 hover:bg-pink-600 text-white font-bold py-3 px-4 rounded focus:outline-none focus:shadow-outline w-full"
              type="submit"
              disabled={isLoading}
            >
              {isLoading ? 'SIGNING IN...' : 'SIGN IN'}
            </button>
          </div>
        </form>
        <p className="text-center text-gray-500 text-sm">
          Not registered? <Link href="/register" className="text-gray-300 hover:text-white">Register now</Link>
        </p>
      </div>
    </div>
  );
};

export default LoginForm;