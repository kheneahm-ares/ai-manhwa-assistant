import { type RouteConfig, route, index } from "@react-router/dev/routes";

export default [
  index("./routes/_index.tsx"),
  route("login", "./routes/login.tsx"),
  route("register", "./routes/register.tsx"),
  route("home", "./routes/home.tsx"),
  route("logout", "./routes/logout.tsx"),
  route("library", "./routes/library.tsx"),
] satisfies RouteConfig;