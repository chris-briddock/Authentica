export interface IUpdateAddressRequest {
    address: {
      name?: string;
      number?: string;
      street: string;
      city: string;
      state: string;
      postcode: string;
      country: string;
    };
  }