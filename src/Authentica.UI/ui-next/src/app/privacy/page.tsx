import React from 'react';
import Link from 'next/link';

const PrivacyPolicy: React.FC = () => {
  return (
    <div className="min-h-screen bg-gray-900 text-gray-300 py-12 px-4 sm:px-6 lg:px-8">
      <div className="max-w-3xl mx-auto">
        <h1 className="text-4xl font-bold text-pink-500 mb-8">Privacy Policy</h1>
        
        <section className="mb-8">
          <h2 className="text-2xl font-semibold text-white mb-4">1. Introduction</h2>
          <p>Welcome to Authentica&apos;s Privacy Policy. This policy describes how Authentica (&quot;we&quot;, &quot;our&quot;, or &quot;us&quot;) collects, uses, and protects your personal information when you use our OAuth 2.0 authentication services.</p>
        </section>

        <section className="mb-8">
          <h2 className="text-2xl font-semibold text-white mb-4">2. Information We Collect</h2>
          <p>We collect the following types of information:</p>
          <ul className="list-disc list-inside ml-4 mt-2">
            <li>Personal information (such as name, email address, phone number)</li>
            <li>Authentication data (such as usernames and encrypted passwords)</li>
            <li>Usage data (such as login times and IP addresses)</li>
            <li>Device information (such as browser type and operating system)</li>
          </ul>
        </section>

        <section className="mb-8">
          <h2 className="text-2xl font-semibold text-white mb-4">3. How We Use Your Information</h2>
          <p>We use your information to:</p>
          <ul className="list-disc list-inside ml-4 mt-2">
            <li>Provide and maintain our authentication services</li>
            <li>Improve and personalize user experience</li>
            <li>Communicate with you about our services</li>
            <li>Detect and prevent fraud and security incidents</li>
          </ul>
        </section>

        <section className="mb-8">
          <h2 className="text-2xl font-semibold text-white mb-4">4. Data Sharing and Disclosure</h2>
          <p>We may share your information with:</p>
          <ul className="list-disc list-inside ml-4 mt-2">
            <li>Third-party service providers that help us operate our services</li>
            <li>Law enforcement or other governmental entities when required by law</li>
            <li>Other parties with your consent or at your direction</li>
          </ul>
        </section>

        <section className="mb-8">
          <h2 className="text-2xl font-semibold text-white mb-4">5. Data Security</h2>
          <p>We implement appropriate technical and organizational measures to protect your personal information against unauthorized or unlawful processing, accidental loss, destruction or damage.</p>
        </section>

        <section className="mb-8">
          <h2 className="text-2xl font-semibold text-white mb-4">6. Your Rights</h2>
          <p>Depending on your location, you may have certain rights regarding your personal information, including:</p>
          <ul className="list-disc list-inside ml-4 mt-2">
            <li>The right to access your personal information</li>
            <li>The right to rectify inaccurate personal information</li>
            <li>The right to erase your personal information</li>
            <li>The right to restrict processing of your personal information</li>
            <li>The right to data portability</li>
          </ul>
        </section>

        <section className="mb-8">
          <h2 className="text-2xl font-semibold text-white mb-4">7. Changes to This Policy</h2>
          <p>We may update this privacy policy from time to time. We will notify you of any changes by posting the new privacy policy on this page and updating the &apos;Last updated&apos; date.</p>
        </section>

        <p className="mt-8 text-sm">Last updated: Sept 2024</p>

        <div className="mt-12">
          <Link href="/" className="text-pink-500 hover:text-pink-400">
            ← Back to Home
          </Link>
        </div>
      </div>
    </div>
  );
};

export default PrivacyPolicy;