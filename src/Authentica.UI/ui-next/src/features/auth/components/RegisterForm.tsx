'use client'

import React, { useState } from 'react';
import { Mail, Lock, Eye, EyeOff, Phone } from 'lucide-react';
import { useForm } from '@/common/hooks/UseForm';
import Link from 'next/link';
import { useRouter } from 'next/navigation';
import { RegisterRequest } from '@/features/auth/services/RegisterRequest';
import { SendConfirmEmailTokenRequest } from '@/features/auth/services/SendConfirmEmailTokenRequest';
import { IRegisterFormRequest } from '@/common/domain/requests/IRegisterFormRequest';
import { IRegisterRequest } from '@/features/auth/requests/IRegisterRequest';
import { ITokenRequest } from '@/features/auth/requests/ITokenRequest';

const RegisterForm: React.FC = () => {
  const router = useRouter();
  const initialState: IRegisterFormRequest = {
    email: '',
    phoneNumber: '',
    password: '',
    confirmPassword: '',
  };

  const { formData, handleFormChange } = useForm<IRegisterFormRequest>(initialState);
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const onSubmit = async () => {
    setIsSubmitting(true);
    try {
      const registerData: IRegisterRequest = {
        email: formData.email,
        phoneNumber: formData.phoneNumber,
        password: formData.password,
      };

      const response = await RegisterRequest(registerData);
      
      if (response.ok) {
        console.log('Registration successful');
        
        // Send token request
        const tokenRequest: ITokenRequest = {
          email: formData.email,
          tokenType: 'confirm_email'
        };
        
        const tokenResponse = await SendConfirmEmailTokenRequest(tokenRequest);
        
        if (tokenResponse.ok) {
          console.log('Confirmation email sent successfully');
          // Redirect to email confirmation page
          router.push('/send-email-confirmation');
        } else {
          console.error('Failed to send confirmation email');
          // You might want to show an error message to the user
        }
      } else {
        // Handle registration error
        const errorData = await response.json();
        console.error('Registration failed:', errorData);
        // You might want to show an error message to the user
      }
    } catch (error) {
      console.error('Registration error:', error);
      // Handle any network or other errors
    } finally {
      setIsSubmitting(false);
    }
  }

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (formData.password !== formData.confirmPassword) {
      alert("Passwords don't match!");
      return;
    }
    onSubmit();
  };

  return (
    <div className="flex items-center justify-center min-h-screen bg-gray-900">
      <div className="w-full max-w-md">
        <form
          onSubmit={handleSubmit}
          className="bg-gray-800 shadow-md rounded px-8 pt-6 pb-8 mb-8 mt-8"
        >
          <div className="mb-4 relative">
            <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
              <Mail className="h-5 w-5 text-gray-500" />
            </div>
            <input
              className="bg-gray-700 appearance-none border-0 rounded w-full py-3 px-3 pl-10 text-gray-300 leading-tight focus:outline-none focus:ring-2 focus:ring-pink-500 focus:ring-opacity-50"
              id="email"
              name="email"
              type="email"
              placeholder="Email"
              value={formData.email}
              onChange={handleFormChange}
              required
            />
          </div>
          <div className="mb-4 relative">
            <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
              <Phone className="h-5 w-5 text-gray-500" />
            </div>
            <input
              className="bg-gray-700 appearance-none border-0 rounded w-full py-3 px-3 pl-10 text-gray-300 leading-tight focus:outline-none focus:ring-2 focus:ring-pink-500 focus:ring-opacity-50"
              id="phoneNumber"
              name="phoneNumber"
              type="tel"
              placeholder="Phone Number"
              value={formData.phoneNumber}
              onChange={handleFormChange}
              required
            />
          </div>
          <div className="mb-4 relative">
            <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
              <Lock className="h-5 w-5 text-gray-500" />
            </div>
            <input
              className="bg-gray-700 appearance-none border-0 rounded w-full py-3 px-3 pl-10 pr-10 text-gray-300 leading-tight focus:outline-none focus:ring-2 focus:ring-pink-500 focus:ring-opacity-50"
              id="password"
              name="password"
              type={showPassword ? "text" : "password"}
              placeholder="Password"
              value={formData.password}
              onChange={handleFormChange}
              required
            />
            <div className="absolute inset-y-0 right-0 pr-3 flex items-center">
              <button
                type="button"
                onClick={() => setShowPassword(!showPassword)}
                className="text-gray-500 focus:outline-none focus:text-white"
              >
                {showPassword ? (
                  <EyeOff className="h-5 w-5" />
                ) : (
                  <Eye className="h-5 w-5" />
                )}
              </button>
            </div>
          </div>
          <div className="mb-4 relative">
            <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
              <Lock className="h-5 w-5 text-gray-500" />
            </div>
            <input
              className="bg-gray-700 appearance-none border-0 rounded w-full py-3 px-3 pl-10 pr-10 text-gray-300 leading-tight focus:outline-none focus:ring-2 focus:ring-pink-500 focus:ring-opacity-50"
              id="confirmPassword"
              name="confirmPassword"
              type={showConfirmPassword ? "text" : "password"}
              placeholder="Confirm Password"
              value={formData.confirmPassword}
              onChange={handleFormChange}
              required
            />
            <div className="absolute inset-y-0 right-0 pr-3 flex items-center">
              <button
                type="button"
                onClick={() => setShowConfirmPassword(!showConfirmPassword)}
                className="text-gray-500 focus:outline-none focus:text-white"
              >
                {showConfirmPassword ? (
                  <EyeOff className="h-5 w-5" />
                ) : (
                  <Eye className="h-5 w-5" />
                )}
              </button>
            </div>
          </div>
          <div className="flex items-center justify-between">
            <button
              className="bg-pink-500 hover:bg-pink-600 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline w-full"
              type="submit"
              disabled={isSubmitting}
            >
              {isSubmitting ? "REGISTERING..." : "REGISTER"}
            </button>
          </div>
          <p className="text-center text-gray-500 text-xs my-4">
            Already have an account?{" "}
            <Link href="/login" className="text-pink-500 hover:text-pink-400">
              Log in
            </Link>
          </p>
        </form>
      </div>
    </div>
  );
};

export default RegisterForm;