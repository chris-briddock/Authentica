'use client'

import React, { useState } from 'react';
import { Mail, Key } from 'lucide-react';
import { useRouter } from 'next/navigation';
import Link from 'next/link';
import { VerifyEmailRequest } from '@/features/auth/services/VerifyEmailRequest';
import { IVerifyEmailRequest } from '@/features/auth/requests/IVerifyEmailRequest';
import Alert from '@/common/components/alert/Alert';  // Import the new Alert component

const ConfirmEmail: React.FC = () => {
  const [email, setEmail] = useState('');
  const [confirmationCode, setConfirmationCode] = useState('');
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [alert, setAlert] = useState<{ type: 'success' | 'error'; message: string } | null>(null);
  const router = useRouter();

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setAlert(null);
    setIsSubmitting(true);

    // Validate input
    if (!email || !confirmationCode) {
      setAlert({ type: 'error', message: 'Please fill in all fields.' });
      setIsSubmitting(false);
      return;
    }

    if (confirmationCode.length !== 6 || !/^\d+$/.test(confirmationCode)) {
      setAlert({ type: 'error', message: 'Confirmation code must be 6 digits.' });
      setIsSubmitting(false);
      return;
    }

    try {
      const verifyData: IVerifyEmailRequest = {
        email: email,
        token: confirmationCode
      };

      const response = await VerifyEmailRequest(verifyData);
      
      if (response.ok) {
        setAlert({ type: 'success', message: 'Email verified successfully. Redirecting to login...' });
        setTimeout(() => router.push('/login'), 2000);  // Redirect after 2 seconds
      } else {
        const errorData = await response.json();
        setAlert({ type: 'error', message: errorData.message || 'Invalid email or confirmation code.' });
      }
    } catch (error) {
      console.error('Error verifying email:', error);
      setAlert({ type: 'error', message: 'An error occurred. Please try again.' });
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="min-h-screen bg-gray-900 flex flex-col justify-center py-12 sm:px-6 lg:px-8">
      <div className="sm:mx-auto sm:w-full sm:max-w-md">
        <div className="text-center">
          <Mail className="mx-auto h-12 w-12 text-pink-500" />
          <h2 className="mt-6 text-center text-3xl font-extrabold text-white">
            Confirm your email
          </h2>
          <p className="mt-2 text-center text-sm text-gray-400">
            Enter your email and the 6-digit code sent to your inbox
          </p>
        </div>
      </div>

      <div className="mt-8 sm:mx-auto sm:w-full sm:max-w-md">
        <div className="bg-gray-800 py-8 px-4 shadow sm:rounded-lg sm:px-10">
          {alert && (
            <div className="mb-4">
              <Alert type={alert.type} message={alert.message} />
            </div>
          )}

          <form className="space-y-6" onSubmit={handleSubmit}>
            <div>
              <label htmlFor="email" className="block text-sm font-medium text-gray-300">
                Email address
              </label>
              <div className="mt-1 relative rounded-md shadow-sm">
                <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                  <Mail className="h-5 w-5 text-gray-400" aria-hidden="true" />
                </div>
                <input
                  id="email"
                  name="email"
                  type="email"
                  autoComplete="email"
                  required
                  className="bg-gray-700 block w-full pl-10 pr-3 py-2 border border-gray-600 rounded-md leading-5 text-gray-300 placeholder-gray-400 focus:outline-none focus:ring-pink-500 focus:border-pink-500 sm:text-sm"
                  placeholder="you@example.com"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                />
              </div>
            </div>

            <div>
              <label htmlFor="confirmation-code" className="block text-sm font-medium text-gray-300">
                Confirmation code
              </label>
              <div className="mt-1 relative rounded-md shadow-sm">
                <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                  <Key className="h-5 w-5 text-gray-400" aria-hidden="true" />
                </div>
                <input
                  id="confirmation-code"
                  name="confirmation-code"
                  type="text"
                  required
                  className="bg-gray-700 block w-full pl-10 pr-3 py-2 border border-gray-600 rounded-md leading-5 text-gray-300 placeholder-gray-400 focus:outline-none focus:ring-pink-500 focus:border-pink-500 sm:text-sm"
                  placeholder="123456"
                  value={confirmationCode}
                  onChange={(e) => setConfirmationCode(e.target.value)}
                  maxLength={6}
                />
              </div>
            </div>

            <div>
              <button
                type="submit"
                className="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-pink-600 hover:bg-pink-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-pink-500"
                disabled={isSubmitting}
              >
                {isSubmitting ? 'Verifying...' : 'Verify Email'}
              </button>
            </div>
          </form>

          <div className="mt-6 space-y-4">
            <p className="text-sm text-gray-300 text-center">
              Please check your email and enter the confirmation code. If you don&apos;t see the email, check your spam folder.
            </p>
            <Link href="/login" className="w-full flex justify-center py-2 px-4 border border-gray-300 rounded-md shadow-sm text-sm font-medium text-gray-300 bg-gray-700 hover:bg-gray-600 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-pink-500">
              Back to login
            </Link>
            <Link href="/register" className="w-full flex justify-center py-2 px-4 border border-gray-300 rounded-md shadow-sm text-sm font-medium text-gray-300 bg-gray-700 hover:bg-gray-600 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-pink-500">
              Back to register
            </Link>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ConfirmEmail;