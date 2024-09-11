'use client'

import Link from 'next/link';
import React, { useState, useEffect } from 'react';

const CookiesPopup: React.FC = () => {
    const [isVisible, setIsVisible] = useState(false);

    useEffect(() => {
        const hasAcceptedCookies = localStorage.getItem('acceptedCookies');
        if (!hasAcceptedCookies) {
            setIsVisible(true);
        }
    }, []);

    const closePopup = () => {
        setIsVisible(false);
        localStorage.setItem('acceptedCookies', 'true');
    };

    const handleLearnMore = () => {
        window.location.href = '/privacy';
    };

    return (
        <>
            {isVisible && (
                <div className="fixed bottom-4 right-4 bg-gray-800 text-gray-300 p-4 rounded shadow-lg">
                    <p className="text-sm">
                        We use cookies to ensure you get the best experience on our website. By using our website, you agree to our{' '}
                        <Link href="/privacy" className="text-pink-500 hover:text-pink-400 underline">
                            Privacy Policy
                        </Link>
                        .
                    </p>
                    <div className="flex justify-end mt-2">
                        <button
                            onClick={closePopup}
                            className="text-sm text-gray-300 hover:text-white underline cursor-pointer mr-2"
                        >
                            Got it!
                        </button>
                        <button
                            onClick={handleLearnMore}
                            className="text-sm font-semibold py-1 px-3 rounded bg-pink-500 text-white hover:bg-pink-600 focus:outline-none focus:ring-2 focus:ring-pink-500 focus:ring-opacity-50"
                        >
                            Learn More
                        </button>
                    </div>
                </div>
            )}
        </>
    );
};

export default CookiesPopup;