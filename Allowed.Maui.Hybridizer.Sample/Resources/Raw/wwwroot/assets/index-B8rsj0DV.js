var le=Object.defineProperty;var ce=(e,t,n)=>t in e?le(e,t,{enumerable:!0,configurable:!0,writable:!0,value:n}):e[t]=n;var u=(e,t,n)=>ce(e,typeof t!="symbol"?t+"":t,n);(function(){const t=document.createElement("link").relList;if(t&&t.supports&&t.supports("modulepreload"))return;for(const s of document.querySelectorAll('link[rel="modulepreload"]'))i(s);new MutationObserver(s=>{for(const o of s)if(o.type==="childList")for(const r of o.addedNodes)r.tagName==="LINK"&&r.rel==="modulepreload"&&i(r)}).observe(document,{childList:!0,subtree:!0});function n(s){const o={};return s.integrity&&(o.integrity=s.integrity),s.referrerPolicy&&(o.referrerPolicy=s.referrerPolicy),s.crossOrigin==="use-credentials"?o.credentials="include":s.crossOrigin==="anonymous"?o.credentials="omit":o.credentials="same-origin",o}function i(s){if(s.ep)return;s.ep=!0;const o=n(s);fetch(s.href,o)}})();function I(){}function U(e){return e()}function q(){return Object.create(null)}function A(e){e.forEach(U)}function Y(e){return typeof e=="function"}function Q(e,t){return e!=e?t==t:e!==t||e&&typeof e=="object"||typeof e=="function"}function de(e){return Object.keys(e).length===0}function c(e,t){e.appendChild(t)}function X(e,t,n){e.insertBefore(t,n||null)}function j(e){e.parentNode&&e.parentNode.removeChild(e)}function h(e){return document.createElement(e)}function C(e){return document.createTextNode(e)}function y(){return C(" ")}function Z(e,t,n,i){return e.addEventListener(t,n,i),()=>e.removeEventListener(t,n,i)}function G(e,t,n){n==null?e.removeAttribute(t):e.getAttribute(t)!==n&&e.setAttribute(t,n)}function ue(e){return Array.from(e.childNodes)}function ee(e,t){t=""+t,e.data!==t&&(e.data=t)}let L;function M(e){L=e}function te(){if(!L)throw new Error("Function called outside component initialization");return L}function fe(e){te().$$.on_mount.push(e)}function ge(e){te().$$.on_destroy.push(e)}const _=[],J=[];let $=[];const D=[],he=Promise.resolve();let N=!1;function we(){N||(N=!0,he.then(ne))}function V(e){$.push(e)}const R=new Set;let v=0;function ne(){if(v!==0)return;const e=L;do{try{for(;v<_.length;){const t=_[v];v++,M(t),pe(t.$$)}}catch(t){throw _.length=0,v=0,t}for(M(null),_.length=0,v=0;J.length;)J.pop()();for(let t=0;t<$.length;t+=1){const n=$[t];R.has(n)||(R.add(n),n())}$.length=0}while(_.length);for(;D.length;)D.pop()();N=!1,R.clear(),M(e)}function pe(e){if(e.fragment!==null){e.update(),A(e.before_update);const t=e.dirty;e.dirty=[-1],e.fragment&&e.fragment.p(e.ctx,t),e.after_update.forEach(V)}}function me(e){const t=[],n=[];$.forEach(i=>e.indexOf(i)===-1?t.push(i):n.push(i)),n.forEach(i=>i()),$=t}const P=new Set;let be;function ie(e,t){e&&e.i&&(P.delete(e),e.i(t))}function ye(e,t,n,i){if(e&&e.o){if(P.has(e))return;P.add(e),be.c.push(()=>{P.delete(e)}),e.o(t)}}function ve(e){e&&e.c()}function se(e,t,n){const{fragment:i,after_update:s}=e.$$;i&&i.m(t,n),V(()=>{const o=e.$$.on_mount.map(U).filter(Y);e.$$.on_destroy?e.$$.on_destroy.push(...o):A(o),e.$$.on_mount=[]}),s.forEach(V)}function re(e,t){const n=e.$$;n.fragment!==null&&(me(n.after_update),A(n.on_destroy),n.fragment&&n.fragment.d(t),n.on_destroy=n.fragment=null,n.ctx=[])}function _e(e,t){e.$$.dirty[0]===-1&&(_.push(e),we(),e.$$.dirty.fill(0)),e.$$.dirty[t/31|0]|=1<<t%31}function oe(e,t,n,i,s,o,r=null,a=[-1]){const d=L;M(e);const l=e.$$={fragment:null,ctx:[],props:o,update:I,not_equal:s,bound:q(),on_mount:[],on_destroy:[],on_disconnect:[],before_update:[],after_update:[],context:new Map(t.context||(d?d.$$.context:[])),callbacks:q(),dirty:a,skip_bound:!1,root:t.target||d.$$.root};r&&r(l.root);let w=!1;if(l.ctx=n?n(e,t.props||{},(g,k,...x)=>{const b=x.length?x[0]:k;return l.ctx&&s(l.ctx[g],l.ctx[g]=b)&&(!l.skip_bound&&l.bound[g]&&l.bound[g](b),w&&_e(e,g)),k}):[],l.update(),w=!0,A(l.before_update),l.fragment=i?i(l.ctx):!1,t.target){if(t.hydrate){const g=ue(t.target);l.fragment&&l.fragment.l(g),g.forEach(j)}else l.fragment&&l.fragment.c();t.intro&&ie(e.$$.fragment),se(e,t.target,t.anchor),ne()}M(d)}class ae{constructor(){u(this,"$$");u(this,"$$set")}$destroy(){re(this,1),this.$destroy=I}$on(t,n){if(!Y(n))return I;const i=this.$$.callbacks[t]||(this.$$.callbacks[t]=[]);return i.push(n),()=>{const s=i.indexOf(n);s!==-1&&i.splice(s,1)}}$set(t){this.$$set&&!de(t)&&(this.$$.skip_bound=!0,this.$$set(t),this.$$.skip_bound=!1)}}const $e="4";typeof window<"u"&&(window.__svelte||(window.__svelte={v:new Set})).v.add($e);const ke="data:image/svg+xml,%3csvg%20xmlns='http://www.w3.org/2000/svg'%20xmlns:xlink='http://www.w3.org/1999/xlink'%20aria-hidden='true'%20role='img'%20class='iconify%20iconify--logos'%20width='26.6'%20height='32'%20preserveAspectRatio='xMidYMid%20meet'%20viewBox='0%200%20256%20308'%3e%3cpath%20fill='%23FF3E00'%20d='M239.682%2040.707C211.113-.182%20154.69-12.301%20113.895%2013.69L42.247%2059.356a82.198%2082.198%200%200%200-37.135%2055.056a86.566%2086.566%200%200%200%208.536%2055.576a82.425%2082.425%200%200%200-12.296%2030.719a87.596%2087.596%200%200%200%2014.964%2066.244c28.574%2040.893%2084.997%2053.007%20125.787%2027.016l71.648-45.664a82.182%2082.182%200%200%200%2037.135-55.057a86.601%2086.601%200%200%200-8.53-55.577a82.409%2082.409%200%200%200%2012.29-30.718a87.573%2087.573%200%200%200-14.963-66.244'%3e%3c/path%3e%3cpath%20fill='%23FFF'%20d='M106.889%20270.841c-23.102%206.007-47.497-3.036-61.103-22.648a52.685%2052.685%200%200%201-9.003-39.85a49.978%2049.978%200%200%201%201.713-6.693l1.35-4.115l3.671%202.697a92.447%2092.447%200%200%200%2028.036%2014.007l2.663.808l-.245%202.659a16.067%2016.067%200%200%200%202.89%2010.656a17.143%2017.143%200%200%200%2018.397%206.828a15.786%2015.786%200%200%200%204.403-1.935l71.67-45.672a14.922%2014.922%200%200%200%206.734-9.977a15.923%2015.923%200%200%200-2.713-12.011a17.156%2017.156%200%200%200-18.404-6.832a15.78%2015.78%200%200%200-4.396%201.933l-27.35%2017.434a52.298%2052.298%200%200%201-14.553%206.391c-23.101%206.007-47.497-3.036-61.101-22.649a52.681%2052.681%200%200%201-9.004-39.849a49.428%2049.428%200%200%201%2022.34-33.114l71.664-45.677a52.218%2052.218%200%200%201%2014.563-6.398c23.101-6.007%2047.497%203.036%2061.101%2022.648a52.685%2052.685%200%200%201%209.004%2039.85a50.559%2050.559%200%200%201-1.713%206.692l-1.35%204.116l-3.67-2.693a92.373%2092.373%200%200%200-28.037-14.013l-2.664-.809l.246-2.658a16.099%2016.099%200%200%200-2.89-10.656a17.143%2017.143%200%200%200-18.398-6.828a15.786%2015.786%200%200%200-4.402%201.935l-71.67%2045.674a14.898%2014.898%200%200%200-6.73%209.975a15.9%2015.9%200%200%200%202.709%2012.012a17.156%2017.156%200%200%200%2018.404%206.832a15.841%2015.841%200%200%200%204.402-1.935l27.345-17.427a52.147%2052.147%200%200%201%2014.552-6.397c23.101-6.006%2047.497%203.037%2061.102%2022.65a52.681%2052.681%200%200%201%209.003%2039.848a49.453%2049.453%200%200%201-22.34%2033.12l-71.664%2045.673a52.218%2052.218%200%200%201-14.563%206.398'%3e%3c/path%3e%3c/svg%3e",xe="/vite.svg";function Se(e){let t,n,i,s,o;return{c(){t=h("button"),n=C("count is "),i=C(e[0])},m(r,a){X(r,t,a),c(t,n),c(t,i),s||(o=Z(t,"click",e[1]),s=!0)},p(r,[a]){a&1&&ee(i,r[0])},i:I,o:I,d(r){r&&j(t),s=!1,o()}}}function Me(e,t,n){let i=0;return[i,()=>{n(0,i+=1)}]}class Ie extends ae{constructor(t){super(),oe(this,t,Me,Se,Q,{})}}class Le{constructor(){u(this,"taskResolvers",{});u(this,"methods",{})}initialize(){function t(n){const i=new CustomEvent("HybridWebViewMessageReceived",{detail:{message:n}});window.dispatchEvent(i)}window.chrome&&window.chrome.webview?window.chrome.webview.addEventListener("message",n=>{t(n.data)}):window.webkit&&window.webkit.messageHandlers&&window.webkit.messageHandlers.webwindowinterop?window.external={receiveMessage:n=>{t(n)}}:window.addEventListener("message",n=>{t(n.data)}),window.addEventListener("HybridWebViewMessageReceived",async n=>{console.log(n.detail.message);const i=n.detail.message.split("|"),s=i[0],o=i.length>1?i.slice(1).join("|"):"";if(s=="Plugin"){const r=JSON.parse(o);this.taskResolvers[r.taskId](r.payload)}else if(s=="Call"){const r=JSON.parse(o);let a;if(this.methods[r.method]){const d=this.methods[r.method](r.payload);a=d instanceof Promise?await d:d}this.callback(r.taskId,a)}})}sendMessage(t){this.sendMessageInternal("RawMessage",t)}sendMessageInternal(t,n){const i=`${t}|${n}`;window.chrome&&window.chrome.webview?window.chrome.webview.postMessage(i):window.webkit&&window.webkit.messageHandlers&&window.webkit.messageHandlers.webwindowinterop?window.webkit.messageHandlers.webwindowinterop.postMessage(i):window.hybridWebViewHost.sendMessage(i)}async invoke(t,n,i=void 0){return await this.invokeRequest(new Pe({plugin:t,method:n,payload:i}))}async invokeRequest(t){return new Promise((n,i)=>{this.taskResolvers[t.taskId]=n,this.sendMessage(`Plugin|${JSON.stringify(t)}`)})}register(t,n){this.methods[t]=n}unregister(t){const n={};for(let i in this.methods)i!==t&&(n[i]=this.methods[i]);this.methods=n}callback(t,n=void 0){this.sendMessage(`Call|${JSON.stringify(new Ce({taskId:t,payload:n}))}`)}}let Oe=0;class Pe{constructor(t){u(this,"plugin");u(this,"method");u(this,"taskId");u(this,"payload");Object.assign(this,t),this.taskId=Oe++}}class Ce{constructor(t){u(this,"taskId");u(this,"payload");Object.assign(this,t)}}const f=new Le;var m=(e=>(e.Web="Web",e.Android="Android",e.IOS="iOS",e))(m||{});class Ee{constructor(){u(this,"platform")}isWeb(){return!this.isAndroid()&&!this.isIOS()}isAndroid(){return typeof window.hybridWebViewHost<"u"}isIOS(){return typeof window.webkit<"u"&&typeof window.webkit.messageHandlers<"u"&&typeof window.webkit.messageHandlers.webwindowinterop<"u"}getPlatform(){return this.isAndroid()?m.Android:this.isIOS()?m.IOS:m.Web}}const E=new Ee;class Ae{async show(t,n,i,s,o){E.getPlatform()==m.Web?alert(n):await f.invoke("Alert","Show",{title:t,message:n,accept:i,cancel:s,flowDirection:o})}}const We=new Ae;class Be{async initialize(){E.getPlatform()!=m.Web&&await f.invoke("ClientLifetime","Initialize")}async dispose(){E.getPlatform()!=m.Web&&await f.invoke("ClientLifetime","Dispose")}}const K=new Be;class He{async getInfo(){if(E.getPlatform()!=m.Web)return await f.invoke("Battery","GetInfo")}}const Re=new He;function Ne(e){let t,n,i,s,o,r,a,d,l,w,g,k,x,b,T,W,F,O,S,B,z;return w=new Ie({}),{c(){t=h("main"),n=h("div"),n.innerHTML=`<a href="https://vitejs.dev" target="_blank" rel="noreferrer"><img src="${xe}" class="logo svelte-1k7epd2" alt="Vite Logo"/></a> <a href="https://svelte.dev" target="_blank" rel="noreferrer"><img src="${ke}" class="logo svelte svelte-1k7epd2" alt="Svelte Logo"/></a>`,i=y(),s=h("h1"),s.textContent="Vite + Svelte",o=y(),r=h("div"),a=h("p"),d=C(e[0]),l=y(),ve(w.$$.fragment),g=y(),k=h("hr"),x=y(),b=h("button"),b.textContent="show alert",T=y(),W=h("p"),W.innerHTML=`Check out <a href="https://github.com/sveltejs/kit#readme" target="_blank" rel="noreferrer">SvelteKit</a>, the
        official Svelte app framework powered by Vite!`,F=y(),O=h("p"),O.textContent="Click on the Vite and Svelte logos to learn more",G(r,"class","card"),G(O,"class","read-the-docs svelte-1k7epd2")},m(p,H){X(p,t,H),c(t,n),c(t,i),c(t,s),c(t,o),c(t,r),c(r,a),c(a,d),c(r,l),se(w,r,null),c(r,g),c(r,k),c(r,x),c(r,b),c(t,T),c(t,W),c(t,F),c(t,O),S=!0,B||(z=Z(b,"click",e[1]),B=!0)},p(p,[H]){(!S||H&1)&&ee(d,p[0])},i(p){S||(ie(w.$$.fragment,p),S=!0)},o(p){ye(w.$$.fragment,p),S=!1},d(p){p&&j(t),re(w),B=!1,z()}}}function Ve(e,t,n){let i="Loading...";function s(r){let a="Battery state could not be determined.";switch(r.state){case 1:a="Battery is actively being charged by a power source.";break;case 2:a="Battery is not plugged in and discharging.";break;case 3:a="Battery is full.";break;case 4:a="Battery is not charging or discharging, but in an in-between state.";break;case 5:a="Battery does not exist on the device.";break}n(0,i=`${a} ${r.chargeLevel*100}%!!!`)}return fe(async()=>{await K.initialize();const r=await Re.getInfo();s(r),f.register("Resumed",()=>{console.log("Hello World!")}),f.register("Stopped",()=>{console.log("Goodbye World!")}),f.register("GetWebViewInfo",()=>({userAgent:navigator.userAgent}))}),ge(()=>{f.unregister("Resumed"),f.unregister("Stopped"),f.unregister("GetWebViewInfo"),f.unregister("BatteryInfoChanged"),K.dispose()}),[i,()=>We.show("Title","Message","Accept","Cancel",2)]}class je extends ae{constructor(t){super(),oe(this,t,Ve,Ne,Q,{})}}f.initialize();new je({target:document.getElementById("app")});
