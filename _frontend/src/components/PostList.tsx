import React from "react";
import { useState } from "react";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "./ui/card";

const POSTS_URL = "/api/posts/";
// let POST_URL = (id: number) => `/api/posts/${id}`;

interface Post {
  id: number;
  ticker: string;
  body: string;
  sentiment: string;
}

const PostList = () => {
  const [isPostListFetched, setIsPostListFetched] = useState(false);
  const [posts, setPosts] = useState<Post[] | undefined>([]);

  async function getAllPosts() {
    try {
      const res = await fetch(POSTS_URL);
      const postsObject = await res.json();
      const posts: Post[] = Object.values(postsObject);
      setPosts(posts);
      console.log(posts);
    } catch (err) {
      console.error(err);
    }
  }

  function handleShowPostsClick() {
    setIsPostListFetched(true);
    getAllPosts();
  }

  function handleHidePostsClick() {
    setIsPostListFetched(false);
  }

  if (!isPostListFetched) {
    return (
      <>
        <button onClick={handleShowPostsClick}>
          Click here to retrieve all the posts.
        </button>
      </>
    );
  } else {
    // return (
    //   <>
    //     {posts?.map((post) => (
    //       <div key={post.id}>
    //         <Card>
    //           <CardHeader>
    //             <CardTitle>{post.ticker}</CardTitle>
    //             <CardDescription>{post.body}</CardDescription>
    //           </CardHeader>
    //           <CardContent className="grid gap-4">
    //             <div className=" flex items-center space-x-4 rounded-md border p-4">
    //               <div className="flex-1 space-y-1">
    //                 <p className="text-sm font-medium leading-none">
    //                   {post.sentiment}
    //                 </p>
    //               </div>
    //             </div>
    //           </CardContent>
    //         </Card>
    //         <br />
    //         <br />
    //         <br />
    //         <br />
    //       </div>
    //     ))}
    //     <button onClick={handleHidePostsClick}>Click here to revert.</button>
    //   </>
    // );
    return (
      <>
        <button onClick={handleHidePostsClick}>Click here to revert.</button>
      </>
    );
  }
};

export default PostList;
