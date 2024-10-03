import { NextResponse } from "next/server";

const API_POSTS_URL = "http://localhost:5138/api/v1/Posts";

// get all posts
export async function GET(request: Request) {
  const requestOptions = {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
  };

  const response = await fetch(API_POSTS_URL, requestOptions);
  const responseData = await response.json();

  return NextResponse.json({ ...responseData }, { status: 200 });
}

// create a post
export async function POST(request: Request) {
  // process incoming form submission request from client
  const requestData = await request.json();
  const formAsJson = JSON.stringify(requestData);

  const requestOptions = {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Accept: "application/json",
    },
    body: formAsJson
  };

  const response = await fetch(API_POSTS_URL, requestOptions);
  console.log(response.status);
  const responseData = await response.json();

  return NextResponse.json({ ...responseData }, { status: 200 });
}
