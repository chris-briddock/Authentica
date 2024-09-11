import React from 'react';
import { Shield, Lock, Users, RefreshCcw, Key, Zap } from 'lucide-react';

const Hero: React.FC = () => {
  return (
    <div className="bg-gray-900 text-white">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-24">
        <div className="text-center">
          <h1 className="text-4xl tracking-tight font-extrabold sm:text-5xl md:text-6xl">
            <span className="block">Secure Authentication</span>
            <span className="block text-pink-500">for the Modern Web</span>
          </h1>
          <p className="mt-3 max-w-md mx-auto text-base text-gray-300 sm:text-lg md:mt-5 md:text-xl md:max-w-3xl">
            Authentica provides robust, scalable, and user-friendly OAuth 2.0 authentication solutions for your applications.
          </p>
          <div className="mt-10 flex justify-center">
            <div className="mt-3 rounded-md shadow sm:mt-0 sm:ml-3">
              <button className="w-full flex items-center justify-center px-8 py-3 border border-transparent text-base font-medium rounded-md text-white bg-pink-600 hover:bg-pink-700 md:py-4 md:text-lg md:px-10">
                Get Started
              </button>
            </div>
          </div>
        </div>
        
        <div className="mt-24">
          <div className="grid grid-cols-1 gap-8 sm:grid-cols-2 lg:grid-cols-3">
            <div className="pt-6">
              <div className="flow-root bg-gray-800 rounded-lg px-6 pb-8">
                <div className="-mt-6">
                  <div>
                    <span className="inline-flex items-center justify-center p-3 bg-pink-500 rounded-md shadow-lg">
                      <Shield className="h-6 w-6 text-white" aria-hidden="true" />
                    </span>
                  </div>
                  <h3 className="mt-8 text-lg font-medium text-white tracking-tight">OAuth 2.0 Compliant</h3>
                  <p className="mt-5 text-base text-gray-400">
                    Fully compliant with OAuth 2.0 specifications, ensuring industry-standard security and interoperability.
                  </p>
                </div>
              </div>
            </div>
            <div className="pt-6">
              <div className="flow-root bg-gray-800 rounded-lg px-6 pb-8">
                <div className="-mt-6">
                  <div>
                    <span className="inline-flex items-center justify-center p-3 bg-pink-500 rounded-md shadow-lg">
                      <Lock className="h-6 w-6 text-white" aria-hidden="true" />
                    </span>
                  </div>
                  <h3 className="mt-8 text-lg font-medium text-white tracking-tight">Multiple Grant Types</h3>
                  <p className="mt-5 text-base text-gray-400">
                    Support for various OAuth 2.0 grant types including Authorization Code, Device Code, Refresh Token, Client Credentials flows.
                  </p>
                </div>
              </div>
            </div>
            <div className="pt-6">
              <div className="flow-root bg-gray-800 rounded-lg px-6 pb-8">
                <div className="-mt-6">
                  <div>
                    <span className="inline-flex items-center justify-center p-3 bg-pink-500 rounded-md shadow-lg">
                      <Users className="h-6 w-6 text-white" aria-hidden="true" />
                    </span>
                  </div>
                  <h3 className="mt-8 text-lg font-medium text-white tracking-tight">Single Sign-On (SSO)</h3>
                  <p className="mt-5 text-base text-gray-400">
                    Enable seamless authentication across multiple applications with our SSO capabilities.
                  </p>
                </div>
              </div>
            </div>
            <div className="pt-6">
              <div className="flow-root bg-gray-800 rounded-lg px-6 pb-8">
                <div className="-mt-6">
                  <div>
                    <span className="inline-flex items-center justify-center p-3 bg-pink-500 rounded-md shadow-lg">
                      <RefreshCcw className="h-6 w-6 text-white" aria-hidden="true" />
                    </span>
                  </div>
                  <h3 className="mt-8 text-lg font-medium text-white tracking-tight">Token Management</h3>
                  <p className="mt-5 text-base text-gray-400">
                    Efficient handling of access tokens, refresh tokens, and token expiration for enhanced security.
                  </p>
                </div>
              </div>
            </div>
            <div className="pt-6">
              <div className="flow-root bg-gray-800 rounded-lg px-6 pb-8">
                <div className="-mt-6">
                  <div>
                    <span className="inline-flex items-center justify-center p-3 bg-pink-500 rounded-md shadow-lg">
                      <Key className="h-6 w-6 text-white" aria-hidden="true" />
                    </span>
                  </div>
                  <h3 className="mt-8 text-lg font-medium text-white tracking-tight">Scope-based Access</h3>
                  <p className="mt-5 text-base text-gray-400">
                    Fine-grained access control with customizable scopes to limit and manage user permissions effectively.
                  </p>
                </div>
              </div>
            </div>
            <div className="pt-6">
              <div className="flow-root bg-gray-800 rounded-lg px-6 pb-8">
                <div className="-mt-6">
                  <div>
                    <span className="inline-flex items-center justify-center p-3 bg-pink-500 rounded-md shadow-lg">
                      <Zap className="h-6 w-6 text-white" aria-hidden="true" />
                    </span>
                  </div>
                  <h3 className="mt-8 text-lg font-medium text-white tracking-tight">Easy Integration</h3>
                  <p className="mt-5 text-base text-gray-400">
                    Seamlessly integrate with your existing applications using our comprehensive SDK and detailed documentation.
                  </p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Hero;