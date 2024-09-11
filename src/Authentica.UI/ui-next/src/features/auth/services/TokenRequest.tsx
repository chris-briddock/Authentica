import { BASE_URL } from "@/common/constants/BaseUrl";

export const TokenRequest = async (): Promise<Response> => {
  const URL: string = `${BASE_URL}/oauth2/token`;
  
  const formData = new URLSearchParams();
  formData.append('grant_type', 'client_credentials');
  formData.append('client_id', process.env.NEXT_CLIENT_ID as string || "1d10a20b-d00c-4cdf-b9a6-c15aec7fb94c");
  formData.append('client_secret', process.env.NEXT_CLIENT_SECRET as string || "b0ahmqtOMNVTnTUJ4E19QNSFe8UYOHqDoO9ovXbNSnRrHjrMbYc1gREBqFOL8XZXuEDFhGamf4Teq7HfXqjMm4kLqjGCg7XAqCjDdUaPSm2HCS2hEL8wR2zD");

  const response = await fetch(URL, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/x-www-form-urlencoded',
    },
    body: formData,
  });

  return response;
}