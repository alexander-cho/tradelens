import { NextResponse } from "next/server";

const API_POST_URL = "http://localhost:5138/api/v1/Posts/1";

export async function GET(request: Request) {
  const requestOptions = {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
  };

  const response = await fetch(API_POST_URL, requestOptions);
  const responseData = await response.json();

  return NextResponse.json({ ...responseData }, { status: 200 });
}
