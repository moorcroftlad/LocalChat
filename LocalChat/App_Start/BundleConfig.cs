using System.Collections.Generic;
using System.Web.Optimization;

namespace LocalChat.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts/global") {Orderer = new NonOrderingBundleOrderer()}.Include(
                "~/assets/js/lib/jquery.min.js",
                "~/assets/js/lib/jquery.signalR-2.2.0.min.js",
                "~/assets/js/lib/handlebars-v2.0.0.js",
                "~/assets/js/shared/pubSub.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts/chat").Include(
                "~/assets/js/chat/chat.js"));

            bundles.Add(new StyleBundle("~/bundles/styles/global").Include(
                "~/assets/bootstrap/css/bootstrap.css",
                "~/assets/css/global/all.css"));
        }
    }

    internal class NonOrderingBundleOrderer : IBundleOrderer
    {
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }
}