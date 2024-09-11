'use client'

import React from 'react';
import { Mail } from 'lucide-react';
import Link from 'next/link';

const SendEmailConfirmation: React.FC = () => {

  return (
    <div className="min-h-screen bg-gray-900 flex flex-col justify-center py-12 sm:px-6 lg:px-8">
      <div className="sm:mx-auto sm:w-full sm:max-w-md">
        <div className="text-center">
          <Mail className="mx-auto h-12 w-12 text-pink-500" />
          <h2 className="mt-6 text-center text-3xl font-extrabold text-white">
            Check your email
          </h2>
          <p className="mt-2 text-center text-sm text-gray-400">
            We&apos;ve sent a confirmation email to your inbox.
          </p>
        </div>
      </div>

      <div className="mt-8 sm:mx-auto sm:w-full sm:max-w-md">
        <div className="bg-gray-800 py-8 px-4 shadow sm:rounded-lg sm:px-10">
          <div className="space-y-6">
            <div>
              <p className="text-sm text-gray-300">
                Please check your email and click on the confirmation link to verify your account. If you don&apos;t see the email, check your spam folder.
              </p>
            </div>
            <div>
              <Link href="/confirm-email" className="w-full flex justify-center py-2 px-4 border border-gray-300 rounded-md shadow-sm text-sm font-medium text-gray-300 bg-gray-700 hover:bg-gray-600 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-pink-500">
                Continue to confirm email
              </Link>
            </div>
            <div>
              <Link href="/login" className="w-full flex justify-center py-2 px-4 border border-gray-300 rounded-md shadow-sm text-sm font-medium text-gray-300 bg-gray-700 hover:bg-gray-600 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-pink-500">
                Back to login
              </Link>
            </div>
            <div>
              <Link href="/register" className="w-full flex justify-center py-2 px-4 border border-gray-300 rounded-md shadow-sm text-sm font-medium text-gray-300 bg-gray-700 hover:bg-gray-600 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-pink-500">
                Back to register
              </Link>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default SendEmailConfirmation;