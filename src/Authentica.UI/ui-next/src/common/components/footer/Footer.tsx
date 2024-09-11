import React from 'react';

const Footer: React.FC = () => {
  return (
    <footer className="bg-gray-800 text-gray-300">
      <div className="max-w-7xl mx-auto py-12 px-4 sm:px-6 lg:py-6 lg:px-8">
          <p className="mt-8 text-base text-gray-400 md:mt-0 md:order-1 text-center">
            &copy; 2024 Authentica. All rights reserved.
          </p>
        </div>
    </footer>
  );
};

export default Footer;